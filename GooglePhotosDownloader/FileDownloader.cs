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

        public async Task SaveFileAsync(MediaItem item, string outputFolder)
        {
            var response = await _client.GetAsync(item.DownloadUrl);
            var content = await response.Content.ReadAsByteArrayAsync();
            var timestamp = item.MediaMetadata.CreationTime.Substring(0, 10).Replace('-', '_');
            var year = timestamp.Substring(0, 4);
            var type = item.MediaMetadata.Video != null ? "Videos" : "Pictures";
            var fileName = $"{timestamp}_{item.Filename}";
            Directory.CreateDirectory(Path.Combine(outputFolder, type, year));
            await File.WriteAllBytesAsync(Path.Combine(outputFolder, type, year, fileName), content);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
