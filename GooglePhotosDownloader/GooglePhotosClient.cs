using GooglePhotosDownloader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GooglePhotosDownloader
{
    public class GooglePhotosClient : IDisposable
    {
        private HttpClient _client;

        public GooglePhotosClient(string accessToken)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://photoslibrary.googleapis.com/v1/");
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                 
        }

        //GET /v1/mediaItems?pageSize=100 ?pageToken=    
        public async Task<IEnumerable<string>> GetFullPhotoListAsync()
        {
            var photoList = new List<string>();
            var response = await _client.GetAsync("mediaItems?pageSize=100");

            var content = await response.Content.ReadAsStringAsync();

            var responseObj = JsonConvert.DeserializeObject<ListMediaItemsResponse>(content);

            foreach (var item in responseObj.MediaItems)
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.BaseUrl);
                photoList.Add(item.BaseUrl);
            }

            return photoList;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}