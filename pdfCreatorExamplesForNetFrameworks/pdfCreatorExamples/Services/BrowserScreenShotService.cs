using System;
using System.IO;
using System.Threading.Tasks;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace pdfCreatorExamples.Services
{
    public static class BrowserScreenShotService
    {
        public static async Task<string> TakePhotoAndReturnImagePath(string chartHtmlBody, string chartId)
        {
            // Configure PuppeteerSharp
            IPage page = await ConfigurePuppeteerSharpAsync();

            // Set the HTML content of the page
            await page.SetContentAsync(chartHtmlBody);

            // Wait for the chart to render
            await page.WaitForSelectorAsync(chartId);

            // Capture a screenshot of the chart canvas
            byte[] screenshot = await TakeScreenShotAsync(page, chartId);

            // Save the screenshot as an image file
            string imagePath = $"{Guid.NewGuid()}.png";
            File.WriteAllBytes(imagePath, screenshot);

            return imagePath;
        }

        private static async Task<IPage> ConfigurePuppeteerSharpAsync()
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            return await browser.NewPageAsync();
        }

        private async static Task<byte[]> TakeScreenShotAsync(IPage page, string chartId)
        {
            // Get the bounding box of the chart canvas
            var chartCanvas = await page.QuerySelectorAsync(chartId);
            var boundingBox = await chartCanvas.BoundingBoxAsync();

            // Capture a screenshot of the chart canvas
            return await page.ScreenshotDataAsync(new ScreenshotOptions
            {
                Clip = new Clip
                {
                    X = (int)boundingBox.X,
                    Y = (int)boundingBox.Y,
                    Width = (int)boundingBox.Width,
                    Height = (int)boundingBox.Height
                }
            });
        }
    }
}
