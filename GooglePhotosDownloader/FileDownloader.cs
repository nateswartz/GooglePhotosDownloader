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
        private string _outputDir;

        public FileDownloader(string outputDir)
        {
            _client = new HttpClient();
            _outputDir = outputDir;
        }

        public bool FileExists(MediaItem item)
        {
            if (File.Exists(GetFullDestinationPath(item)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task SaveFileAsync(MediaItem item)
        {
            Directory.CreateDirectory(GetDestinationFolder(item));

            var response = await _client.GetAsync(item.DownloadUrl);
            var content = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(GetFullDestinationPath(item), content);  
        }

        private string GetFullDestinationPath(MediaItem item)
        {
            var dir = GetDestinationFolder(item);
            var timestamp = item.MediaMetadata.CreationTime.Substring(0, 10).Replace('-', '_');
            var fileName = $"{timestamp}_{item.Filename}";
            return Path.Combine(dir, fileName);
        }

        private string GetDestinationFolder(MediaItem item)
        {
            var year = item.MediaMetadata.CreationTime.Substring(0, 4);
            var type = item.MediaMetadata.Video != null ? "Videos" : "Pictures";
            return Path.Combine(_outputDir, type, year);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
