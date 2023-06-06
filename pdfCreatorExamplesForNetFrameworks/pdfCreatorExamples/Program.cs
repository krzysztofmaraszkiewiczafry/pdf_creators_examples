using System.Threading.Tasks;
using pdfCreatorExamples.Libraries;

namespace pdfCreatorExamples
{
    internal class Program
    {
        static async Task Main()
        {
            await SelectPdfCreator.CreatePdfAsync();
        }
    }
}
