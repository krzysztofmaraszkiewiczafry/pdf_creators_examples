using System;
using System.Drawing.Printing;
using System.IO;
using System.Threading.Tasks;
using pdfCreatorExamples.Services;
using TuesPechkin;

namespace pdfCreatorExamples.Libraries.Creators
{
    public class TuesPechkinPdfCreator : PdfCreatorBase
    {
        protected override async Task<string> GeneratePdfDocumentAndReturnPdfFilePathAsync(string pdfSuffixNameWithExtension, string chartHtmlBody)
        {
            string pdfPath = GetPdfFilePath(pdfSuffixNameWithExtension);
            string chartScreenShotFilePath = await BrowserScreenShotService.TakePhotoAndReturnImagePath(chartHtmlBody, "#myChart");

            Console.WriteLine(chartScreenShotFilePath);

            var htmlBodyToConvert = $"<h1>Pretty Websites</h1><p>This might take a bit to convert!</p> " +
                $"<img src='./{chartScreenShotFilePath}' />";

            var document = new HtmlToPdfDocument
            {
                GlobalSettings =
                {
                    ProduceOutline = true,
                    DocumentTitle = "Pretty Websites",
                    PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
                    Margins =
                    {
                        All = 1.375,
                        Unit = Unit.Centimeters
                    }
                },
                Objects = {
                    new ObjectSettings { HtmlText = htmlBodyToConvert }
                }
            };

            byte[] pdfDocument = null;
                //covert html to pdf
                IConverter converter = new ThreadSafeConverter(
                                            new PdfToolset(
                                                new WinAnyCPUEmbeddedDeployment(
                                                    new TempFolderDeployment())));

                pdfDocument = converter.Convert(document);


            File.WriteAllBytes(pdfPath, pdfDocument);

            return pdfPath;
        }
    }
}
