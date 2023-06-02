using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            //{ "GoogleCharts_vertical_column_chart", "GoogleCharts\\vertical_column_chart.html" },
            //{ "GoogleCharts_pie_chart", "GoogleCharts\\pie_chart.html" },
            //{ "Highcharts_pie_chart", "Highcharts\\pie_chart.html" },
            //{ "Highcharts_vertical_column_chart", "Highcharts\\vertical_column_chart.html" },
        };

        public void CreatePdf()
        {
            //Console.WriteLine($"Starting genereting demo pdf for sample table and charts for SelectPdfCreator");
            //GeneratePdfFile(nameof(SelectPdfCreator), "demoTemplate.html");

            foreach (KeyValuePair<string, string> chartsDemoHtmlFile in chartsDemoHtmlFiles) 
            {
                Console.WriteLine($"Starting genereting demo pdf file for {chartsDemoHtmlFile.Key} using {chartsDemoHtmlFile.Value} html file");
                GeneratePdfFile(chartsDemoHtmlFile.Key, chartsDemoHtmlFile.Value);
            }
        }

        private static void GeneratePdfFile(string name, string filePath)
        {
            string pdfPath = GetPdfFilePath(name);

            DocumentOneBodyModel parameters = GetDocumentOneBodyModel();
            string htmlBody = SetParametersToHtml(GetHtmlBody(filePath), parameters);

            // instantiate the html to pdf converter
            HtmlToPdf converter = new HtmlToPdf();

            // convert the url to pdf
            PdfDocument doc = converter.ConvertHtmlString(htmlBody);

            // save pdf document
            doc.Save(pdfPath);

            // close pdf document
            doc.Close();
            //open pdf
            Console.WriteLine($"Opening the file {pdfPath}");
            Process.Start(pdfPath);
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
