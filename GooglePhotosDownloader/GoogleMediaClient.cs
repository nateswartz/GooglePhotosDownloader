using GooglePhotosDownloader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GooglePhotosDownloader
{
    public class GoogleMediaClient : IDisposable
    {
        private HttpClient _client;
        private int _pageSize;

        public GoogleMediaClient(string accessToken, int pageSize = 100)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://photoslibrary.googleapis.com/v1/");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            _pageSize = pageSize > 100 ? 100 : pageSize;
        }

        //GET /v1/mediaItems?pageSize=100 ?pageToken=    
        public async Task<IEnumerable<MediaItem>> GetFullMediaListAsync()
        {
            var itemList = new List<MediaItem>();
            var nextPageToken = "";

            do
            {
                var items = await GetMediaListPageAsync(nextPageToken);
                if (items.mediaItems == null)
                {
                    break;
                }
                itemList.AddRange(items.mediaItems);
                nextPageToken = items.nextPageToken;

                Console.WriteLine($"Got items - {nextPageToken}");

            } while (!string.IsNullOrEmpty(nextPageToken));

            return itemList;
        }

        public async Task<(string nextPageToken, IEnumerable<MediaItem> mediaItems)> GetMediaListPageAsync(string nextPageToken = "")
        {
            var pageTokenArg = string.IsNullOrEmpty(nextPageToken) ? nextPageToken : $"&pageToken={nextPageToken}";
            var response = await _client.GetAsync($"mediaItems?pageSize={_pageSize}{pageTokenArg}");

            var content = await response.Content.ReadAsStringAsync();
            var responseObj = JsonConvert.DeserializeObject<ListMediaItemsResponse>(content);

            return (responseObj.NextPageToken, responseObj.MediaItems);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}