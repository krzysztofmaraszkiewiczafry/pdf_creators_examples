using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DinkToPdf;

namespace pdfCreatorExamples.Libraries
{
    /// <summary>
    /// Not working for .net 4.8, this solution is working only for .net core frameworks
    /// </summary>
    public class DinkToPdfPdfCreator : PdfCreatorBase//, IPdfCreator
    {
        public void CreatePdf()
        {
            string pdfPath = GeneratePdfFile(nameof(DinkToPdfPdfCreator));

            //create pdf converter
            var converter = new SynchronizedConverter(new PdfTools());

            HtmlToPdfDocument htmlToPdfDocument = GetHtmlToPdfDocument();

            byte[] pdfFileInBytes = converter.Convert(htmlToPdfDocument);

            File.WriteAllBytes(pdfPath, pdfFileInBytes);
            Process.Start(pdfPath);
        }

        private static HtmlToPdfDocument GetHtmlToPdfDocument()
        {
            var objectSettings = new ObjectSettings
            {
                HeaderSettings = { FontName = "Arial", FontSize = 9, Line = false },
                FooterSettings = { FontName = "Arial", FontSize = 7, Center = "www.afry.com", Right = "Sida [page] / [toPage]", Line = false },
                HtmlContent = GetHtmlBody()
            };

            var globalSettings = new GlobalSettings
            {
                Margins = new MarginSettings { Top = 45, Left = 20, Right = 20, Bottom = 20 }
            };

            var pdf = new HtmlToPdfDocument
            {
                //GlobalSettings = globalSettings
            };

            pdf.Objects.AddRange(new List<DinkToPdf.ObjectSettings>
            {
                objectSettings
            });

            return pdf;
        }

        private static string GetHtmlBody() =>
            File.ReadAllText("./Templates/demo.html");
    }
}
