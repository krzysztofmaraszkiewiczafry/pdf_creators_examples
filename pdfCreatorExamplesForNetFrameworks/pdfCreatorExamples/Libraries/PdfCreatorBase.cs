using System;
using System.IO;

namespace pdfCreatorExamples.Libraries
{
    public class PdfCreatorBase
    {
        protected static string GetPdfFilePath(string fileNameSuffix)
        {
            string pdfDirectoryPath = GeneratePdfDirectory();

            return Path.Combine(pdfDirectoryPath, $"{fileNameSuffix}_{Guid.NewGuid()}.pdf");
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
