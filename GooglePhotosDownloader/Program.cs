using System;
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

            using (var authClient = new GoogleAuthClient(clientId, clientSecret))
            {
                var accessToken = await authClient.GetAuthTokenAsync(refreshToken);

                using (var mediaClient = new GoogleMediaClient(accessToken))
                {
                    var items = await mediaClient.GetFullMediaListAsync();
                    Console.Write(items.Count());

                    var fileDownloader = new FileDownloader();
                    foreach (var item in items)
                    {
                        await fileDownloader.SaveFileAsync(item);
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}