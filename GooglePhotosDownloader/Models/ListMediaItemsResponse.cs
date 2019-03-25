using Newtonsoft.Json;
using System.Collections.Generic;

namespace GooglePhotosDownloader.Models
{
    public class ListMediaItemsResponse
    {
        [JsonProperty("mediaItems")]
        public List<MediaItem> MediaItems { get; set; }
        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }
    }

    public class MediaItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("productUrl")]
        public string ProductUrl { get; set; }
        [JsonProperty("baseUrl")]
        public string BaseUrl { get; set; }
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }
        [JsonProperty("mediaMetadata")]
        public MediaMetadata MediaMetadata { get; set; }
        [JsonProperty("contributorInfo")]
        public ContributorInfo ContributorInfo { get; set; }
        [JsonProperty("filename")]
        public string Filename { get; set; }

        public string DownloadUrl
        {
            get
            {
                if (MediaMetadata.Photo != null)
                {
                    return $"{BaseUrl}=d";
                }
                else
                {
                    return $"{BaseUrl}=dv";
                }
            } 
        }
    }

    public class MediaMetadata
    {
        [JsonProperty("creationTime")]
        public string CreationTime { get; set; }
        [JsonProperty("width")]
        public string Width { get; set; }
        [JsonProperty("height")]
        public string Height { get; set; }
        [JsonProperty("photo")]
        public Photo Photo { get; set; }
        [JsonProperty("video")]
        public Video Video { get; set; }
    }

    public class Photo
    {
        [JsonProperty("cameraMake")]
        public string CameraMake { get; set; }
        [JsonProperty("cameraModel")]
        public string CameraModel { get; set; }
        [JsonProperty("focalLength")]
        public string FocalLength { get; set; }
        [JsonProperty("apertureFNumber")]
        public string ApertureFNumber { get; set; }
        [JsonProperty("isoEquivalent")]
        public string IsoEquivalent { get; set; }
        [JsonProperty("exposureTime")]
        public string ExposureTime { get; set; }
    }

    public class Video
    {
        [JsonProperty("cameraMake")]
        public string CameraMake { get; set; }
        [JsonProperty("cameraModel")]
        public string CameraModel { get; set; }
        [JsonProperty("fps")]
        public string FPS { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class ContributorInfo
    {
        [JsonProperty("profilePictureBaseUrl")]
        public string ProfilePictureBaseUrl { get; set; }
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}