using System.Threading.Tasks;

namespace pdfCreatorExamples.Libraries
{
    public interface IPdfCreator
    {
        Task CreatePdfAsync();
    }
}
