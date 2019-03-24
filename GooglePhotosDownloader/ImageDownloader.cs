using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Threading.Tasks;

namespace GooglePhotosDownloader
{
    public class ImageLoader
    {
        private HttpClient _client;

        public ImageLoader()
        {
            _client = new HttpClient();
        }

        public async Task SaveImageAsync(string filename, string uri)
        {
            var response = await _client.GetAsync(uri);
            var content = await response.Content.ReadAsStreamAsync();
            using (var image = new Bitmap(content))
            {
                image.Save($"{filename}.jpg", ImageFormat.Jpeg);
            }
        }
    }
}
