﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GooglePhotosDownloader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json")
                                            .Build();

            var clientId = config["ClientId"];
            var clientSecret = config["ClientSecret"];
            var refreshTokens = config.GetSection("RefreshTokens").Get<string[]>();
            var outputDir = config["OutputDir"];

            var tasks = new List<Task>();

            foreach (var token in refreshTokens)
            {
                tasks.Add(DownloadPhotosForAccount(clientId, clientSecret, token, outputDir));
            }
            await Task.WhenAll(tasks);
        }

        private static async Task DownloadPhotosForAccount(string clientId, string clientSecret, string refreshToken, string outputDir)
        {
            using (var authClient = new GoogleAuthClient(clientId, clientSecret))
            {
                var accessToken = await authClient.GetAuthTokenAsync(refreshToken);
                
                using (var mediaClient = new GoogleMediaClient(accessToken))
                {
                    Console.WriteLine($"Getting items for token {refreshToken}...");

                    var items = await mediaClient.GetFullMediaListAsync();

                    Console.WriteLine($"Found {items.Count()} items. Beginning download...");

                    using (var fileDownloader = new FileDownloader(outputDir))
                    {
                        foreach (var item in items)
                        {            
                            if (!fileDownloader.FileExists(item))
                            {
                                Console.WriteLine($"Saving {item.Filename}...");
                                await fileDownloader.SaveFileAsync(item);
                            }
                            else
                            {
                                Console.WriteLine($"Skipping {item.Filename}, file already exists...");
                            }
                        }
                    }
                    Console.ReadLine();
                }
            }
        }
    }
}
