using GooglePhotosDownloader.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GooglePhotosDownloader
{
    public class FileDownloader : IDisposable
    {
        private HttpClient _client;

        public FileDownloader()
        {
            _client = new HttpClient();
        }

        public async Task SaveFileIfNewAsync(MediaItem item, string outputFolder)
        {
            var timestamp = item.MediaMetadata.CreationTime.Substring(0, 10).Replace('-', '_');
            var year = timestamp.Substring(0, 4);
            var type = item.MediaMetadata.Video != null ? "Videos" : "Pictures";
            var fileName = $"{timestamp}_{item.Filename}";
            var destination = Path.Combine(outputFolder, type, year);
            Directory.CreateDirectory(destination);

            if (!File.Exists(Path.Combine(destination, fileName)))
            {
                var response = await _client.GetAsync(item.DownloadUrl);
                var content = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(Path.Combine(destination, fileName), content);
            }
            else
            {
                Console.WriteLine($"Skipping {item.Filename}, file already exists...");
            }
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
