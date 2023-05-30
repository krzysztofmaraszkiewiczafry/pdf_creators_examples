using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using pdfCreatorExamples.Libraries;

namespace pdfCreatorExamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WkhtmltoxLoader.Load();

            var types = Assembly.GetEntryAssembly().GetTypes();

            var pdfCreatorTypes = types.Where(
                t => t.GetInterfaces().Any(i => i == typeof(IPdfCreator)) && !t.IsInterface);

            foreach ( Type pdfCreatorType in pdfCreatorTypes)
            {
                Console.WriteLine($"Generate pdf using {pdfCreatorType.Name}");

                object instance = Activator.CreateInstance(pdfCreatorType);

                MethodInfo createPdfMethod = pdfCreatorType.GetMethod(nameof(IPdfCreator.CreatePdf));
                createPdfMethod.Invoke(instance, null);
                Console.WriteLine($"========================================");
            }
        }
    }
}
