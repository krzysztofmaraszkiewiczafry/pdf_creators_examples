using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using pdfCreatorExamples.Services;

namespace pdfCreatorExamples.Libraries
{
    public abstract class PdfCreatorBase
    {
        private static object Locker = new object();

        //demo html files with example of charts from 3 diffrent free libraries
        protected static readonly Dictionary<string, string> chartsDemoHtmlFiles = new Dictionary<string, string>
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

        protected async Task GeneratePdfFileAsync(string pdfSuffixNameWithExtension, string chartHtmlFileRelativePath)
        {
            Console.WriteLine($"Starting genereting demo pdf file for {pdfSuffixNameWithExtension} " +
                $"using {chartHtmlFileRelativePath} html file using {GetType().Name}");

            try
            {
                string chartHtmlBody = GetHtmlBody(chartHtmlFileRelativePath);

                string pdfPath = await GeneratePdfDocumentAndReturnPdfFilePathAsync(pdfSuffixNameWithExtension, chartHtmlBody);

                //open pdf
                Console.WriteLine($"Opening the file {pdfPath}");
                Process.Start(pdfPath);

            }
            catch (Exception err)
            {
                Console.WriteLine($"Error: {err.Message}");
            }
        }

        protected abstract Task<string> GeneratePdfDocumentAndReturnPdfFilePathAsync(string pdfSuffixNameWithExtension, string chartHtmlBody);

        protected static string SetParametersToHtml(string htmlBody, DocumentOneBodyModel parameters)
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

        protected static DocumentOneBodyModel GetDocumentOneBodyModel()
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

        protected static string GetHtmlBody(string path) =>
            File.ReadAllText(Path.Combine("Templates", path));

        protected string GetPdfFilePath(string fileNameSuffix)
        {
            string pdfDirectoryPath = GeneratePdfDirectory();
            string typeName = GetType().Name;

            return Path.Combine(pdfDirectoryPath, $"{typeName}_{fileNameSuffix}_{Guid.NewGuid()}.pdf");
        }

        private static string GeneratePdfDirectory()
        {
            string pdfDirectoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "pdfFiles");

            if (!Directory.Exists(pdfDirectoryPath))
            {
                Directory.CreateDirectory(pdfDirectoryPath);
            }

            return pdfDirectoryPath;
        }
    }
}
