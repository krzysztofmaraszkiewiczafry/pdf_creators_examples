using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using pdfCreatorExamples.Libraries;
using pdfCreatorExamples.Libraries.Creators;

namespace pdfCreatorExamples
{
    internal class Program
    {
        static async Task Main()
        {
            var tasks = new List<Task>
            {
                new SelectPdfCreator().CreatePdfAsync(),
                new TuesPechkinPdfCreator().CreatePdfAsync()
            };

            await Task.WhenAll(tasks);

            Console.ReadLine();
        }
    }
}
