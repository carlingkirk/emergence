using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Emergence.Service.Extensions;
using Emergence.Service.Interfaces;
using ExifLib;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Emergence.Service
{
    public class BlobService : IBlobService
    {
        private readonly string _connectionString;

        public BlobService(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:BlobStorage"];
        }

        public async Task<IBlobResult> UploadPhotoAsync(IFormFile photo, string type, string userId, string name)
        {
            var typeContainerClient = new BlobContainerClient(_connectionString, type.ToLower());
            await typeContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            HttpStatusCode status;
            var photoClient = typeContainerClient.GetBlobClient(name);
            using (var stream = photo.OpenReadStream())
            {
                var result = await photoClient.UploadAsync(stream);
                status = (HttpStatusCode)result.GetRawResponse().Status;
            }

            if (status == HttpStatusCode.Created)
            {
                var blobProperties = await SetBlobProperties(photoClient, photo, userId);

                return blobProperties;
            }

            return null;
        }

        public async Task<IEnumerable<IBlobResult>> UploadPhotosAsync(IEnumerable<IFormFile> photos, string type, string userId, string name)
        {
            var results = new List<BlobResult>();
            var typeContainerClient = new BlobContainerClient(_connectionString, type.ToLower());
            await typeContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            foreach (var photo in photos)
            {
                var result = await UploadPhotoAsync(photo, type, userId, name);
                results.Add((BlobResult)result);
            }
            return results;
        }

        private async Task<IBlobResult> SetBlobProperties(BlobClient client, IFormFile file, string userId)
        {
            Dictionary<string, string> metadata = null;
            using (var stream = file.OpenReadStream())
            using (var reader = new ExifReader(stream))
            {
                metadata = GetMetadata(reader);
            }
            metadata.Add("UserId", userId);

            await client.SetMetadataAsync(metadata);
            var metadataResult = client.GetProperties().Value;
            return new BlobResult
            {
                Metadata = metadataResult.Metadata,
                ContentType = metadataResult.ContentType
            };
        }

        private Dictionary<string, string> GetMetadata(ExifReader reader)
        {
            var metadata = new Dictionary<string, string>();
            var latitude = reader.GetLatitude();
            var longitude = reader.GetLongitude();
            var altitude = reader.GetAltitude();
            var length = reader.GetLength();
            var width = reader.GetWidth();
            var dateTaken = reader.GetDateTaken();

            if (latitude.HasValue)
            {
                metadata.Add("Latitude", latitude.Value.ToString());
            }
            if (longitude.HasValue)
            {
                metadata.Add("Longitude", longitude.Value.ToString());
            }
            if (dateTaken.HasValue)
            {
                metadata.Add("DateTaken", dateTaken.Value.ToString());
            }
            if (altitude.HasValue)
            {
                metadata.Add("Altitude", altitude.Value.ToString());
            }
            if (length.HasValue)
            {
                metadata.Add("Length", length.Value.ToString());
            }
            if (width.HasValue)
            {
                metadata.Add("Width", width.Value.ToString());
            }

            return metadata;
        }
    }
}
