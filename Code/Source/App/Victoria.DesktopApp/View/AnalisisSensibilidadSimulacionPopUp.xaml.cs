using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Victoria.Shared.EventArgs;
using Victoria.UI.SharedWPF;
using Victoria.ViewModelWPF;
using iTextSharp;
using iTextSharp.text.pdf;
using System.Data;
using System.IO;
using iTextSharp.text;
using Victoria.Shared;
using Victoria.Shared.AnalisisPrevio;
using System.Collections.ObjectModel;
using Victoria.DesktopApp.Helpers;
//using excel = Microsoft.Office.Interop.Excel;
using ClosedXML.Excel;
using ClosedXML;

namespace Victoria.DesktopApp.View
{
    /// <summary>
    /// Interaction logic for CloseDialog.xaml
    /// </summary>
    public partial class AnalisisSensibilidadSimulacionPopUp : Window
    {
        private IList<StageViewModelBase> stages;

        private AnalisisSensibilidadPopUp analisisSensibilidadPopUp;
        private List<string> vicPaths;

        public DialogResult Result { get; set; }
        public bool IsSimulationOpen { get; set; }
        private string simulationPath { get; set; }
        private int StagesFinished { get; set; }
        private bool simulationStoped { get; set; }
        private DateTime simulationStartedTime { get; set; }
        private TimeSpan simulationTotalTime { get; set; }
        private double barraProgresoValue { get; set; }
        private bool conTiempoFinal { get; set; }
        
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);
        private UpdateProgressBarDelegate updatePbDelegate { get; set; }
        
        public AnalisisSensibilidadSimulacionPopUp(string simulationPath, List<string> vicPaths, AnalisisSensibilidadPopUp analisisSensibilidadPopUp)
        {
            InitializeComponent();
            this.simulationPath = simulationPath;
            this.simulationStoped = false;
            this.vicPaths = vicPaths;
            this.analisisSensibilidadPopUp = analisisSensibilidadPopUp;
            this.StagesFinished = 0;
            double totalValue = 0;
            this.stages = new ObservableCollection<StageViewModelBase>();
            this.simulationStartedTime = DateTime.Now;
            this.conTiempoFinal = false;

            foreach (string vicPath in vicPaths) {
                string simulationFile = File.ReadAllText(vicPath);
                var simulation = XMLParser.GetSimulation(simulationFile);
                if (simulation.Stages.Any())
                {
                    StageViewModel stage = new StageViewModel(simulation) { Name = simulation.Stages.First().Name };
                    this.stages.Add(stage);
                    Variable tiempoFinal = stage.Simulation.GetVariables().First(v => v.Name == "TF");
                    if (tiempoFinal != null && tiempoFinal.InitialValue > 0)
                    {
                        this.conTiempoFinal = true;
                        totalValue = totalValue + stage.Simulation.GetVariables().First(v => v.Name == "TF").InitialValue;
                        stage.Variables.First(v => v.Name == "T").PropertyChanged += VariableTChanged;
                    }
                    stage.Simulation.SimulationStatusChanged += OnSimulationStatusChanged;
                    //stage.ExecuteStageCommand.Execute(null);
                }
            }
            if (this.conTiempoFinal)
            {
                barraProgreso.Minimum = 0;
                barraProgreso.Maximum = totalValue;
                barraProgreso.Value = 0;
                this.barraProgresoValue = 0;
                this.updatePbDelegate = new UpdateProgressBarDelegate(barraProgreso.SetValue);
            }
            else
            {
                barraProgreso.IsIndeterminate = true;
            }
            // Despues que quedó configurada la barra y eso, mando a ejecutar
            foreach (StageViewModel stage in this.stages)
            {
                stage.ExecuteStageCommand.Execute(null);
            }
            
        }

        private void VariableTChanged(object sender, PropertyChangedEventArgs e)
        {
            double value = 0;
            foreach (StageViewModel stage in this.stages)
            {
                value = value + stage.Variables.First(v => v.Name == "T").ActualValue;
            }
            if (value > 0 && value > this.barraProgresoValue)
            {
                this.barraProgresoValue = value;
                Dispatcher.Invoke(updatePbDelegate, System.Windows.Threading.DispatcherPriority.Background, new object[] { ProgressBar.ValueProperty, value });
            }
        }

        private void OnSimulationStatusChanged(object sender, SimulationStatusChangedEventArgs simulationStatusChangedEventArgs)
        {
            switch (simulationStatusChangedEventArgs.Status)
            {
                case SimulationStatus.Stoped:
                    this.StagesFinished += 1;
                    if (this.StagesFinished == this.stages.Count)
                    {
                        this.simulationStoped = true;
                        DateTime now = DateTime.Now;
                        this.simulationTotalTime = now.Subtract(this.simulationStartedTime);
                        
                        // Genero Resultados en formato Excel
                        this.PrintResultsExcel(simulationPath + "\\resultados.xlsx");

                        // GENERO LOS RESULTADOS EN PDF
                        this.PrintResultsPDF(stages);
                        
                        // ABRO LA VENTANA DEL ARCHIVO QUE CONTIENE LOS RESULTADOS
                        System.Diagnostics.Process.Start("explorer.exe", "/select," + simulationPath);
                        // CIERRO LOS POPUPS ABIERTOS
                        analisisSensibilidadPopUp.Dispatcher.Invoke(
                          System.Windows.Threading.DispatcherPriority.Normal,
                          new Action(
                            delegate()
                            {
                                analisisSensibilidadPopUp.Close();
                            }
                        ));
                        this.Dispatcher.Invoke(
                          System.Windows.Threading.DispatcherPriority.Normal,
                          new Action(
                            delegate()
                            {
                                this.Close();
                            }
                        ));
                    }
                    break;
                case SimulationStatus.Started:
                    // asd
                    break;
            }
        }

        private void PrintResultsExcel(String fileName)
        {
            /*
             * REQUERIMIENTO:
             Lo que tiene que contener el archivo es una tabla donde las primeras columnas 
             deben contener cada una de las variables de control y luego las variables de resultado. 
             Cada una de las filas de esa tabla debe contener los valores para cada una de las 
             corridas del análisis de sensibilidad. 
             La primera fila (fila de títulos) debe contener el nombre de cada variable.
            */
            List<DataTable> resultsTable = createResultsTables(stages);

            XLWorkbook oWB = new XLWorkbook();
            var ws = oWB.Worksheets.Add("Resultados");            
            object misvalue = System.Reflection.Missing.Value;            
            int index_col = 1;
            int index_row = 1;
            int index_row_header = 4;
            int cant_escenarios = 0;
            bool tieneTituloVariables = false;

            //Titulo
            ws.Cell(index_row, index_col).Value = "Analisis de Sensibilidad";

            index_row++;

            ws.Row(2).Height = ws.Row(2).Height / 2;
            /*
             * La variable "resultsTable" contiene 2 tablas por cada escenario simulado, cada par de tablas
             * contienen la siguiente SubTabla (en adelante tabla):
             *      - La primer tabla contiene un TableName que representa el nombre del escenario y al 
             *      recorrer las columnas y las filas podemos obtener información de las variables de control.
             *      - La segunda tabla NO contiene un TableName, pero contiene datos de las variables de Resultado 
             *      (que se pueden visualizar recorriendo las filas y columnas).
             */
            foreach (var tbl in resultsTable)
            {                
                if (tbl.TableName != "")
                {
                    //Obtengo información del escenario.
                    cant_escenarios++;
                    index_col = 1;
                    ws.Cell(index_row_header + cant_escenarios, index_col).Value = tbl.TableName;                    
                }
                                                                
                //Datos
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    index_col++;
                    //Setea el nombre de las variables
                    ws.Cell(index_row_header, index_col).Value = tbl.Rows[i][0].ToString();
                    
                    //En laprimer pasada setea el header (combinando celdas) indicando el tipo de variable
                    if (!tieneTituloVariables)
                    {
                        ws.Cell(index_row_header-1 , index_col).Value = tbl.Rows[i].Table.Columns[0].ToString();
                        ws.Range(index_row_header -1, index_col, index_row_header -1, index_col + tbl.Rows.Count - 1).Merge();
                        ws.Cell(index_row_header - 1, index_col).Style.Alignment.WrapText = true;
                        ws.Cell(index_row_header - 1, index_col).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;                        
                        tieneTituloVariables = true;
                    }
                    //Setea el valor que tomaron las variables y lo coloca en filas diferentes en funcion del escenario.
                    ws.Cell(index_row_header + cant_escenarios, index_col).Value = tbl.Rows[i][1].ToString();                    
                }
                tieneTituloVariables = false;
            }

            //Da formato al encabezado
            ws.Range(1, 1, 1, index_col).Style.Fill.BackgroundColor = XLColor.Aquamarine;
            ws.Range(1, 1, 1, index_col).Merge();
            ws.Range(1, 1, 1, index_col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;            
            ws.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;            
            ws.Row(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            //Se coloca el tiempo de ejecución
            int index_tiempo_ejecucion = index_row_header + cant_escenarios + 2;
            ws.Cell(index_tiempo_ejecucion, 1).Value = "Tiempo total de ejecución: " + this.simulationTotalTime.ToString(@"hh\:mm\:ss");
            ws.Range(index_tiempo_ejecucion, 1, index_tiempo_ejecucion, index_col).Merge();

            //Autoajustar al contenido
            ws.Row(3).AdjustToContents(29.0,35.00);
            ws.Columns().AdjustToContents(5.0,100.0);
            
            oWB.SaveAs(fileName);
            
        }       

        private void PrintResultsPDF(IList<StageViewModelBase> stages)
        {
            List<DataTable> resultsTable = createResultsTables(stages);
            
            var filePath = simulationPath + "\\resultados.pdf";            
            System.IO.FileStream fs = new System.IO.FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            // calling PDFFooter class to Include in document
            writer.PageEvent = new PDFFooter();

            document.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntHead = new Font(bfntHead, 16, 1, BaseColor.BLACK);
            iTextSharp.text.Paragraph prgHeading = new iTextSharp.text.Paragraph();
            prgHeading.Alignment = Element.ALIGN_LEFT;
            prgHeading.Add(new Chunk("Análisis de Sensibilidad", fntHead));
            document.Add(prgHeading);

            //Formato de textos
            BaseFont bfntStage = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntStage = new Font(bfntStage, 14, 1, BaseColor.BLACK);
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, 14, 1, BaseColor.WHITE);

            //CARGO LAS TABLAS SEGUN LA CANTIDAD DE STAGES
            foreach (var tbl in resultsTable)
            {
                if (tbl.TableName != "")
                {
                    //LINEA
                    iTextSharp.text.Paragraph p = new iTextSharp.text.Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100, BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
                    document.Add(p);
                    document.Add(new Chunk("\n", fntHead));
                    //NOMBRE DEL STAGE
                    iTextSharp.text.Paragraph stageHeading = new iTextSharp.text.Paragraph();
                    stageHeading.Alignment = Element.ALIGN_CENTER;
                    stageHeading.Add(new Chunk(tbl.TableName, fntStage));
                    document.Add(stageHeading);
                }
                document.Add(new Chunk("\n", fntStage));
                //ESCRIBO TABLA
                PdfPTable table = new iTextSharp.text.pdf.PdfPTable(tbl.Columns.Count);
                //HEADER DE TABLA
                for (int i = 0; i < tbl.Columns.Count; i++)
                {
                    PdfPCell cell = new iTextSharp.text.pdf.PdfPCell();
                    cell.BackgroundColor = new BaseColor(67, 142, 185);
                    cell.AddElement(new Chunk(tbl.Columns[i].ColumnName, fntColumnHeader));
                    table.AddCell(cell);
                }

                //DATOS TABLA
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    for (int j = 0; j < tbl.Columns.Count; j++)
                    {
                        table.AddCell(tbl.Rows[i][j].ToString());
                    }
                }

                document.Add(table);
            }
            //LINEA
            iTextSharp.text.Paragraph line = new iTextSharp.text.Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100, BaseColor.BLACK, Element.ALIGN_LEFT, 0)));
            document.Add(line);
            document.Add(new Chunk("\n", fntHead));
            //Tiempo total
            BaseFont bfntTime = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntTime = new Font(bfntTime, 14, 1, BaseColor.BLACK);
            iTextSharp.text.Paragraph prgTime = new iTextSharp.text.Paragraph();
            prgTime.Alignment = Element.ALIGN_LEFT;
            prgTime.Add(new Chunk("Tiempo total de ejecución: "+this.simulationTotalTime.ToString(@"hh\:mm\:ss"), fntTime));
            document.Add(prgTime);
            
            document.Close();
            writer.Close();
            fs.Close();
        }

        private List<DataTable> createResultsTables(IList<StageViewModelBase> stages)
        {
            List<DataTable> tablesList = new List<DataTable>();

            foreach (var stg in stages)
            {
                DataTable table = new DataTable();
                
                table.TableName = stg.Name;
                table.Columns.Add("Variable de Control");
                table.Columns.Add("Valor");

                DataTable table2 = new DataTable();

                table2.TableName = "";
                table2.Columns.Add("Variable de Resultado");
                table2.Columns.Add("Valor");

                foreach (var variable in stg.Simulation.GetVariables())
                {
                    if (variable.Type == VariableType.Control)
                    {
                        if (variable is VariableArray)
                        {
                            var variableArray = (VariableArray)variable;
                            foreach (var variableAux in variableArray.Variables)
                            {
                                table.Rows.Add(variableAux.Name, variableAux.ActualValue.ToString());
                            }
                        }
                        else
                        {
                            table.Rows.Add(variable.Name, variable.ActualValue.ToString());
                        }
                    }
                    if (variable.Type == VariableType.Result)
                    {
                        if (variable is VariableArray)
                        {
                            var variableArray = (VariableArray)variable;
                            foreach (var variableAux in variableArray.Variables)
                            {
                                table2.Rows.Add(variableAux.Name, variableAux.ActualValue.ToString());
                            }
                        }
                        else
                        {
                            table2.Rows.Add(variable.Name, variable.ActualValue.ToString());
                        }
                    }
                }

                tablesList.Add(table);
                tablesList.Add(table2);
            }

            return tablesList;
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            this.Result = UI.SharedWPF.DialogResult.Cancel;
            foreach (StageViewModel stage in this.stages)
            {
                this.simulationStoped = true;
                stage.StopExecutionStageCommand.Execute(null);
            }
            this.Close();
        }
    }

}
