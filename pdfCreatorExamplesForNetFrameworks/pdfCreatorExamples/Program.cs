using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using pdfCreatorExamples.Libraries;

namespace pdfCreatorExamples
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            WkhtmltoxLoader.Load();

            var types = Assembly.GetEntryAssembly().GetTypes();

            var pdfCreatorTypes = types.Where(
                t => t.GetInterfaces().Any(i => i == typeof(IPdfCreator)) && !t.IsInterface);
            try
            {
                foreach (Type pdfCreatorType in pdfCreatorTypes)
                {
                    Console.WriteLine($"Generate pdf using {pdfCreatorType.Name}");

                    object instance = Activator.CreateInstance(pdfCreatorType);

                    MethodInfo createPdfMethod = pdfCreatorType.GetMethod(nameof(IPdfCreator.CreatePdfAsync));
                    Task invokeTask = (Task)createPdfMethod.Invoke(instance, null);
                    await invokeTask;
                    Console.WriteLine($"========================================");
                }
            }
            catch (Exception err) 
            {
                Console.WriteLine( err.ToString() );
            }

            Console.ReadLine();
        }
    }
}
