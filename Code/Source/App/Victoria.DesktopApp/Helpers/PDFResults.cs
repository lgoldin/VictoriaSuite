using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Victoria.Shared;
using Victoria.Shared.AnalisisPrevio;
using System.Collections.ObjectModel;
using Victoria.DesktopApp.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Victoria.ViewModelWPF;

namespace Victoria.DesktopApp.Helpers
{
    class PDFResults : Results, IResults
    {

        public PDFResults(String _simulationPath,String _fileName, IList<StageViewModelBase> _stages, TimeSpan _simulationTotalTime) 
            : base(_simulationPath, _fileName, _stages,_simulationTotalTime)
        {            
        }

        public void Print()
        {
            
            List<DataTable> resultsTable = createResultsTables(stages);

            //var fileName = simulationPath + "\\resultados.pdf";
            System.IO.FileStream fs = new System.IO.FileStream(simulationPath + "\\" + fileName, FileMode.Create, FileAccess.Write, FileShare.None);
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
            prgTime.Add(new Chunk("Tiempo total de ejecución: " + simulationTotalTime.ToString(@"hh\:mm\:ss"), fntTime));
            document.Add(prgTime);

            document.Close();
            writer.Close();
            fs.Close();
        }
    }
}
