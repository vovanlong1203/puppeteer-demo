using PuppeteerSharp;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var option = new LaunchOptions
            {
                Headless = true,
                Args = new String[]
                {
                    "--no-sandbox",
                    "--disable-setuid-sandbox",
                    "--disable-dev-shm-usage"
                }
            };

            var broswer = await Puppeteer.LaunchAsync(option);

            var page = await broswer.NewPageAsync();

            await page.SetRequestInterceptionAsync(true);

            page.Request += async (sender, e) =>
            {
                var request = e.Request;

                Console.WriteLine("Request URL: " + request.Url);
                Console.WriteLine("Request Method: " + request.Method);

                foreach (var kvp in request.Headers)
                {
                    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
                }
                if (request.PostData != null)
                {
                    Console.WriteLine("Post Data : ");
                    Console.WriteLine(request.PostData);
                }

                await request.ContinueAsync();
            };

            page.Response += async (sender, e) =>
            {
                var response = e.Response;

                Console.WriteLine("\n Respone ---");
                Console.WriteLine("URL: " + response.Url);
                Console.WriteLine("Status: " + response.Status);

                Console.WriteLine("Response Header: ");
                foreach (var kvp in response.Headers)
                {
                    Console.WriteLine($" {kvp.Key}  {kvp.Value}");
                }

                try
                {
                    var body = await response.BufferAsync();
                    Console.WriteLine("Response Body (text, truncated):");
                    Console.WriteLine(Encoding.UTF8.GetString(body).Substring(0, Math.Min(500, body.Length)));

                    var size = body.Length;
                    Console.WriteLine($"Transferred Size (compressed): {size} bytes");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading body: " + ex.Message);
                }

                Console.WriteLine("----------------\n");
            };

            await page.GoToAsync("https://scrapingant.com/blog/web-scraping-puppeteer-sharp");

            await Task.Delay(10000);

            await broswer.CloseAsync();

            Console.ReadLine();
        }

    }
}
