using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Victoria.DesktopApp.Helpers
{
    public class PDFFooter : PdfPageEventHelper
    {
        // write on top of document
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
        }

        // write on start of each page
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            base.OnStartPage(writer, document);

            var parentFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string imageURL = Path.Combine(parentFolder, @"victoria3-5.png");
            var logoPng = Image.GetInstance(imageURL);
            logoPng.ScaleToFit(240, 60);
            //logoPng.Alignment = iTextSharp.text.Image.UNDERLYING;
            logoPng.SetAbsolutePosition(document.Right - 40, document.Top - 28);
            document.Add(logoPng);
        }

        // write on end of each page
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);
            //Escribe fecha y copyright del pdf
            PdfPTable tabFot = new PdfPTable(new float[] { 1F });
            PdfPTable tabFotCopy = new PdfPTable(new float[] { 1F });
            PdfPCell cell;
            PdfPCell cellCopy;

            tabFot.TotalWidth = 300F;
            tabFotCopy.TotalWidth = 300F;

            var fechaHoy = DateTime.Now;
            var footer = fechaHoy.Day.ToString() + "/" + fechaHoy.Month.ToString() + "/" + fechaHoy.Year.ToString();
            var footerCopyright = "Copyright ©2017 - UTN F.R.B.A.";

            cell = new PdfPCell(new Phrase(footer));
            cell.BorderColor = BaseColor.WHITE;
            cellCopy = new PdfPCell(new Phrase(footerCopyright));
            cellCopy.BorderColor = BaseColor.WHITE;

            tabFot.AddCell(cell);
            tabFot.WriteSelectedRows(0, -1, document.Left, document.Bottom, writer.DirectContent);
            tabFot.AddCell(cellCopy);
            tabFot.WriteSelectedRows(1, -1, document.Right - 180, document.Bottom, writer.DirectContent);
        }

        //write on close of document
        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
        }
    } 
}
