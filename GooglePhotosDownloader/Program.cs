﻿using System;
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

                using (var photosClient = new GooglePhotosClient(accessToken))
                {
                    var photos = await photosClient.GetFullPhotoListAsync();
                    Console.Write(photos.Count());

                    var imageDownloader = new ImageLoader();
                    var i = 0;
                    foreach (var photoUrl in photos)
                    {
                        await imageDownloader.SaveImageAsync($"test{i}", photoUrl);
                        i++;
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}