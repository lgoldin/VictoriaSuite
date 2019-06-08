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
    public class ExcelResults : Results, IResults
    {

        public IXLWorksheet ws { get; set; }
        public int index_col { get; set; }
        public int index_row { get; set; }        

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

            List<DataTable> resultsTable = createResultsTables(stages);
            XLWorkbook oWB = new XLWorkbook();
            this.ws = oWB.Worksheets.Add("Resultados");
            //object misvalue = System.Reflection.Missing.Value;
            
            int index_col = 1;
            int index_row = 1;
            int index_row_header = 4;
            int cant_escenarios = 0;
            int maxMergeValue = 1;
            int maxIndexCol = 0;

            Dictionary<String, int> ubi_col_variables = new Dictionary<string, int>();

            bool tieneTituloVariables = false;

            //Insertar Titulo
            ws.Cell(index_row, index_col).Value = "Analisis de Sensibilidad";                       
            index_row++;
            //A la fila siguiente al titulo le reduce la altura.
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
                    /* 
                     * Obtengo información del escenario.
                     * Al recibir un escenario, me posiciono el la primer columna e incremento el subIndice que determina
                     * la cantidad de escenarios.
                     */
                    cant_escenarios++;
                    maxIndexCol = ubi_col_variables.Count + 1 > maxIndexCol ? ubi_col_variables.Count + 1 : maxIndexCol;                    
                    index_col = 1;
                    ws.Cell(index_row_header + cant_escenarios, index_col).Value = tbl.TableName;
                    ws.Cell(index_row_header + cant_escenarios, index_col).Style.Font.SetBold(true);
                    ws.Cell(index_row_header + cant_escenarios, index_col).Style.Font.SetItalic(true);                    
                    SetCellBorder(index_row_header + cant_escenarios, index_col);
                }

                //Datos
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    // Me desplazo una columna a la derecha.
                    index_col++;

                    //En la primer pasada setea el header (combinando celdas) indicando el tipo de variable
                    if (!tieneTituloVariables)
                    {
                        //maxMergeValue = (tbl.Rows.Count - 1) > 8 ? 8 : tbl.Rows.Count - 1;
                        maxMergeValue = ubi_col_variables.Count + 1 > 8 ? 8 : ubi_col_variables.Count + 1;
                        ws.Cell(index_row_header - 1, index_col).Value = tbl.Rows[i].Table.Columns[0].ToString();
                        ws.Range(index_row_header - 1, index_col, index_row_header - 1, index_col + maxMergeValue).Merge();
                        
                        //Setea Formato del encabezado
                        ws.Cell(index_row_header - 1, index_col).Style.Alignment.WrapText = true;
                        ws.Cell(index_row_header - 1, index_col).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;                        
                        SetCellBorder(index_row_header - 1, index_col);
                        ws.Cell(index_row_header - 1, index_col).Style.Font.SetBold(true);
                        ws.Cell(index_row_header - 1, index_col).Style.Font.SetItalic(true);
                        ws.Cell(index_row_header - 1, index_col).Style.Font.SetFontSize(14);

                        tieneTituloVariables = true;
                    }
                    //Setea el nombre de las variables
                    if (ws.Cell(index_row_header, index_col).Value.ToString() == String.Empty)
                    {
                        ws.Cell(index_row_header, index_col).Value = tbl.Rows[i][0].ToString();
                        ws.Cell(index_row_header, index_col).Style.Font.SetBold(true);
                        ws.Cell(index_row_header, index_col).Style.Font.SetItalic(true);
                        ws.Cell(index_row_header, index_col).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        ubi_col_variables.Add(tbl.Rows[i][0].ToString(), index_col);
                    }

                    //Setea el valor que tomaron las variables y lo coloca en filas diferentes en funcion del escenario.                    
                    ubi_col_variables.TryGetValue(tbl.Rows[i][0].ToString(), out int val);
                    ws.Cell(index_row_header + cant_escenarios, val).Value = decimal.Parse(tbl.Rows[i][1].ToString());                    

                    //Setea el valor que tomaron las variables y lo coloca en filas diferentes en funcion del escenario.
                    //ws.Cell(index_row_header + cant_escenarios, index_col).Value = decimal.Parse(tbl.Rows[i][1].ToString(), NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture);
                }
                tieneTituloVariables = false;
            }
            
            maxMergeValue = maxIndexCol >= 10 ? 10 : ubi_col_variables.Count +1 ;
            
            //Da formato al Titulo
            ws.Range(1, 1, 1, maxMergeValue).Style.Fill.BackgroundColor = XLColor.Aquamarine;
            ws.Range(1, 1, 1, maxMergeValue).Merge();
            ws.Range(1, 1, 1, maxMergeValue).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Range(1, 1, 1, maxMergeValue).Style.Font.Bold = true;
            ws.Range(1, 1, 1, maxMergeValue).Style.Font.SetFontSize(16);
            ws.Row(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Row(3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            
            SetCellBorder(ws.Range(index_row_header - 1, 2, index_row_header, ubi_col_variables.Count + 1));
            SetCellBorder(ws.Range(index_row_header + 1, 1, index_row_header + cant_escenarios, ubi_col_variables.Count+1));

            //Se coloca el tiempo de ejecución
            int index_tiempo_ejecucion = index_row_header + cant_escenarios + 2;
            ws.Cell(index_tiempo_ejecucion, 1).Value = "Tiempo total de ejecución: " + this.simulationTotalTime.ToString(@"hh\:mm\:ss");
            ws.Range(index_tiempo_ejecucion, 1, index_tiempo_ejecucion, index_col).Merge();

            //Autoajustar al contenido
            ws.Row(3).AdjustToContents(35.0, 37.00);
            ws.Columns().AdjustToContents(index_row_header + 1,index_row_header + cant_escenarios,8.0, 13.0);
            //ws.Rows().AdjustToContents(1, ubi_col_variables.Count + 1, 5.0, 13.0);
            ws.Column(2).AdjustToContents(15.0, 30.0);            
            ws.Column(1).AdjustToContents(22.0, 30.0);

            oWB.SaveAs(simulationPath + "\\" + fileName);
        }

        public void SetCellBorder(int row, int column)
        {
            ws.Cell(row, column).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            ws.Cell(row, column).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Cell(row, column).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Cell(row, column).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Cell(row, column).Style.Border.BottomBorderColor = XLColor.Black;
            ws.Cell(row, column).Style.Border.LeftBorderColor = XLColor.Black;
            ws.Cell(row, column).Style.Border.RightBorderColor = XLColor.Black;
            ws.Cell(row, column).Style.Border.TopBorderColor = XLColor.Black;
        }

        public void SetCellBorder(IXLRange rango)
        {
            rango.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
            rango.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            rango.Style.Border.RightBorder = XLBorderStyleValues.Thin;
            rango.Style.Border.TopBorder = XLBorderStyleValues.Thin;
            rango.Style.Border.BottomBorderColor = XLColor.Black;
            rango.Style.Border.LeftBorderColor = XLColor.Black;
            rango.Style.Border.RightBorderColor = XLColor.Black;
            rango.Style.Border.TopBorderColor = XLColor.Black;
        }
    }

}
