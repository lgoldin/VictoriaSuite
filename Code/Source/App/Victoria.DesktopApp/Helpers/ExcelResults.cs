using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp.Helpers
{
    class ExcelResults : Results, IResults
    {
        public static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(App));
        public ExcelResults(String _simulationPath, String _fileName, IList<StageViewModelBase> _stages, TimeSpan _simulationTotalTime) 
            : base(_simulationPath, _fileName, _stages, _simulationTotalTime)
        {
        }

        public void Print()
        {
            /*
             * REQUERIMIENTO:
             Lo que tiene que contener el archivo es una tabla donde las primeras columnas 
             deben contener cada una de las variables de control y luego las variables de resultado. 
             Cada una de las filas de esa tabla debe contener los valores para cada una de las 
             corridas del análisis de sensibilidad. 
             La primera fila (fila de títulos) debe contener el nombre de cada variable.
            */
            //logger.Info("Inicio Imprimir Excel");
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
                        ws.Cell(index_row_header - 1, index_col).Value = tbl.Rows[i].Table.Columns[0].ToString();
                        ws.Range(index_row_header - 1, index_col, index_row_header - 1, index_col + tbl.Rows.Count - 1).Merge();
                        ws.Cell(index_row_header - 1, index_col).Style.Alignment.WrapText = true;
                        ws.Cell(index_row_header - 1, index_col).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        tieneTituloVariables = true;
                    }
                    //Setea el valor que tomaron las variables y lo coloca en filas diferentes en funcion del escenario.
                    ws.Cell(index_row_header + cant_escenarios, index_col).Value = decimal.Parse(tbl.Rows[i][1].ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture);
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
            ws.Row(3).AdjustToContents(29.0, 35.00);
            ws.Columns().AdjustToContents(5.0, 100.0);

            oWB.SaveAs(simulationPath + "\\" + fileName);
            //logger.Info("Fin Imprimir Excel");
        }
    }
}
