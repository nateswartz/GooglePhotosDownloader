using GooglePhotosDownloader.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GooglePhotosDownloader
{
    public class GoogleAuthClient : IDisposable
    {
        private HttpClient _client;
        private string _clientId;
        private string _clientSecret;

        public GoogleAuthClient(string clientId, string clientSecret)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://www.googleapis.com/oauth2/v4/");

            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public async Task<string> GetAuthTokenAsync(string refreshToken)
        {
            try
            {
                var parameters = new Dictionary<string, string>
                {
                    {"refresh_token", refreshToken },
                    {"client_id", _clientId },
                    {"client_secret", _clientSecret },
                    {"grant_type", "refresh_token" }
                };

                var postContent = new FormUrlEncodedContent(parameters);

                var response = await _client.PostAsync("token", postContent);

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TokenRefreshResponse>(responseContent).AccessToken;
            }
            catch (Exception e)
            {
                var tmp = e;
                return null;
            }

        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}