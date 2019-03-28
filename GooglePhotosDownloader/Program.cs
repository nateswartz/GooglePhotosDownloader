using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GooglePhotosDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var clientId = "";
            var clientSecret = "";
            var refreshToken = "";
            var outputDir = "test";

            using (var authClient = new GoogleAuthClient(clientId, clientSecret))
            {
                var accessToken = await authClient.GetAuthTokenAsync(refreshToken);

                using (var mediaClient = new GoogleMediaClient(accessToken))
                {
                    var items = await mediaClient.GetFullMediaListAsync();
                    Console.WriteLine($"Found {items.Count()} items");

                    Directory.CreateDirectory(outputDir);

                    var fileDownloader = new FileDownloader();
                    foreach (var item in items)
                    {
                        if (!File.Exists(item.Filename))
                        {
                            await fileDownloader.SaveFileAsync(item, outputDir);
                        }
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}
