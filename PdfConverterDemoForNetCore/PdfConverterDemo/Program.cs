// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using DinkToPdf;
using PdfConverterDemo;

WkhtmltoxLoader.Load();
CreatePdf();

void CreatePdf()
{
    string pdfPath = GeneratePdfFile("DinkToPdfPdfCreator");

    //create pdf converter
    var converter = new SynchronizedConverter(new PdfTools());

    HtmlToPdfDocument htmlToPdfDocument = GetHtmlToPdfDocument();

    byte[] pdfFileInBytes = converter.Convert(htmlToPdfDocument);

    File.WriteAllBytes(pdfPath, pdfFileInBytes);
    try
    {
    Process.Start(pdfPath);
    }
    catch (Exception ex) 
    {
        Console.WriteLine($"Could not open the file, but pdf file create with success. You have to open it manualy");
        Console.WriteLine($"Path of created pdf is here: {pdfPath}");
    }
}

static HtmlToPdfDocument GetHtmlToPdfDocument()
{
    var objectSettings = new ObjectSettings
    {
        HeaderSettings = { FontName = "Arial", FontSize = 9, Line = false },
        FooterSettings = { FontName = "Arial", FontSize = 7, Center = "www.afry.com", Right = "Sida [page] / [toPage]", Line = false },
        HtmlContent = GetHtmlBody()
    };

    var globalSettings = new GlobalSettings
    {
        Margins = new MarginSettings { Top = 45, Left = 20, Right = 20, Bottom = 20 }
    };

    var pdf = new HtmlToPdfDocument
    {
        //GlobalSettings = globalSettings
    };

    pdf.Objects.AddRange(new List<DinkToPdf.ObjectSettings>
            {
                objectSettings
            });

    return pdf;
}

static string GetHtmlBody() =>
    File.ReadAllText("demo.html");


static string GeneratePdfFile(string fileNameSuffix)
{
    string pdfDirectoryPath = GeneratePdfDirectory();

    return Path.Combine(pdfDirectoryPath, $"{fileNameSuffix}_{Guid.NewGuid()}.pdf");
}

static string GeneratePdfDirectory()
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
