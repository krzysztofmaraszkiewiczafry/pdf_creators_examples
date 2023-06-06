using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using pdfCreatorExamples.Services;
using SelectPdf;

namespace pdfCreatorExamples.Libraries
{
    public class SelectPdfCreator : PdfCreatorBase, IPdfCreator
    {
        //demo html files with example of charts from 3 diffrent free libraries
        private readonly Dictionary<string, string> chartsDemoHtmlFiles = new Dictionary<string, string>
        {
            { "Chartjs_pie_chart", "Chart.js\\pie_chart.html" },
            { "Chartjs_vertical_column_chart", "Chart.js\\vertical_column_chart.html" },
            { "GoogleCharts_vertical_column_chart", "GoogleCharts\\vertical_column_chart.html" },
            { "GoogleCharts_pie_chart", "GoogleCharts\\pie_chart.html" },
            { "Highcharts_pie_chart", "Highcharts\\pie_chart.html" },
            { "Highcharts_vertical_column_chart", "Highcharts\\vertical_column_chart.html" },
        };

        public async Task CreatePdfAsync()
        {
            List<Task> tasks = new List<Task>(); 
            foreach (KeyValuePair<string, string> chartsDemoHtmlFile in chartsDemoHtmlFiles) 
            {
                tasks.Add(GeneratePdfFileAsync(chartsDemoHtmlFile.Key, chartsDemoHtmlFile.Value));
            }

            await Task.WhenAll(tasks);
        }

        private static async Task GeneratePdfFileAsync(string pdfSuffixNameWithExtension, string chartHtmlFileRelativePath)
        {
            Console.WriteLine($"Starting genereting demo pdf file for {pdfSuffixNameWithExtension} using {chartHtmlFileRelativePath} html file");

            try
            {
                DocumentOneBodyModel parameters = GetDocumentOneBodyModel();
                string chartHtmlBody = GetHtmlBody(chartHtmlFileRelativePath);
                string mainHtmlBody = SetParametersToHtml(GetHtmlBody("demo.html"), parameters);
                string chartScreenShotFilePath = await BrowserScreenShotService.TakePhotoAndReturnImagePath(chartHtmlBody, "#myChart");

                //set image path to html img node
                mainHtmlBody = mainHtmlBody.Replace("@{ImagePath}", Path.GetFullPath(chartScreenShotFilePath));

                string pdfPath = GeneratePdfDocumentAndReturnPdfFilePath(pdfSuffixNameWithExtension, mainHtmlBody);

                //open pdf
                Console.WriteLine($"Opening the file {pdfPath}");
                Process.Start(pdfPath);

            }
            catch (Exception err)
            {
                Console.WriteLine($"Error: {err.Message}");
            }
        }

        private static string GeneratePdfDocumentAndReturnPdfFilePath(string pdfSuffixNameWithExtension, string htmlToConvert)
        {
            string pdfPath = GetPdfFilePath(pdfSuffixNameWithExtension);

            // Convert the HTML to PDF using SelectPdf
            var converter = new HtmlToPdf();
            converter.Options.PdfPageSize = PdfPageSize.A4;
            converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
            PdfDocument pdfDocument = converter.ConvertHtmlString(htmlToConvert);

            pdfDocument.Save(pdfPath);

            // close pdf document
            pdfDocument.Close();

            return pdfPath;
        }

        private static string GetHtmlBody(string path) =>
            File.ReadAllText(Path.Combine("Templates", path));

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
