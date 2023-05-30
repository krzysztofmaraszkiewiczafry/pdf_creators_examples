using System;
using System.Diagnostics;
using System.IO;
using SelectPdf;

namespace pdfCreatorExamples.Libraries
{
    public class SelectPdfCreator : PdfCreatorBase, IPdfCreator
    {
        public void CreatePdf()
        {
            string pdfPath = GeneratePdfFile(nameof(SelectPdfCreator));

            DocumentOneBodyModel parameters = GetDocumentOneBodyModel();
            string htmlBody = SetParametersToHtml(GetHtmlBody(), parameters);

            // instantiate the html to pdf converter
            HtmlToPdf converter = new HtmlToPdf();

            // convert the url to pdf
            PdfDocument doc = converter.ConvertHtmlString(htmlBody);

            // save pdf document
            doc.Save(pdfPath);

            // close pdf document
            doc.Close();
            //open pdf
            Process.Start(pdfPath);
        }

        private static string GetHtmlBody() =>
            File.ReadAllText("./Templates/demoTemplate.html");

        private static string SetParametersToHtml(string htmlBody, DocumentOneBodyModel parameters)
        {
            htmlBody = htmlBody.Replace("@{ProjectName}", parameters.ProjectName);
            htmlBody = htmlBody.Replace("@{CustomersReferenceId}", parameters.CustomersReferenceId);
            htmlBody = htmlBody.Replace("@{CustomerName}", parameters.CustomerName);
            htmlBody = htmlBody.Replace("@{FreeText}", parameters.FreeText);
            htmlBody = htmlBody.Replace("@{Prerequisites}", parameters.Prerequisites);
            htmlBody = htmlBody.Replace("@{ProjectManager}", parameters.ProjectManager);
            htmlBody = htmlBody.Replace("@{ValidTo}", parameters.ValidTo);
            htmlBody = htmlBody.Replace("@{CreatedDate}", parameters.CreatedDate);

            return htmlBody;
        }

        private static DocumentOneBodyModel GetDocumentOneBodyModel()
        {
            return new DocumentOneBodyModel
            {
                ProjectName = "Project1",
                CustomersReferenceId = "1234",
                CustomerName = "Customer",
                FreeText = "Some free text",
                Prerequisites = "Some prerequisites text",
                ProjectManager = "Project Manager name",
                ValidTo = DateTime.Now.ToShortDateString(),
                CreatedDate = DateTime.Now.ToShortDateString()
            };
        }
    }
}
