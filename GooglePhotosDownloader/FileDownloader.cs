using GooglePhotosDownloader.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GooglePhotosDownloader
{
    public class FileDownloader
    {
        private HttpClient _client;

        public FileDownloader()
        {
            _client = new HttpClient();
        }

        public async Task SaveFileAsync(MediaItem item)
        {
            var response = await _client.GetAsync(item.DownloadUrl);
            var content = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"Saving {item.Filename}");
            await File.WriteAllBytesAsync(item.Filename, content);
        }
    }
}
