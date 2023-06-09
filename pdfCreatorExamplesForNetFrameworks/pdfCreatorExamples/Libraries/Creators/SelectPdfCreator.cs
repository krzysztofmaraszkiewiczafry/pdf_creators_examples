using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using pdfCreatorExamples.Services;
using SelectPdf;

namespace pdfCreatorExamples.Libraries
{
    public class SelectPdfCreator : PdfCreatorBase
    {
        protected override async Task<string> GeneratePdfDocumentAndReturnPdfFilePathAsync(string pdfSuffixNameWithExtension, string chartHtmlBody)
        {
            DocumentOneBodyModel parameters = GetDocumentOneBodyModel();
            string mainHtmlBody = SetParametersToHtml(GetHtmlBody("demo.html"), parameters);
            string pdfPath = GetPdfFilePath(pdfSuffixNameWithExtension);
            string chartScreenShotFilePath = await BrowserScreenShotService.TakePhotoAndReturnImagePath(chartHtmlBody, "#myChart");

            // Convert the HTML to PDF using SelectPdf
            var converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;

            //set image path to html img node
            mainHtmlBody = mainHtmlBody.Replace("@{ImagePath}", Path.GetFullPath(chartScreenShotFilePath));

            PdfDocument pdfDocument = converter.ConvertHtmlString(mainHtmlBody);

            pdfDocument.Save(pdfPath);

            // close pdf document
            pdfDocument.Close();

            return pdfPath;
        }
    }
}
