using System.Collections.Generic;
using System.IO;
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
            _connectionString = configuration["AzureStorageConnectionString"];
        }

        public async Task<IBlobResult> UploadPhotoAsync(IFormFile photo, string userId, string blobPath)
        {
            var typeContainerClient = new BlobContainerClient(_connectionString, "photos");
            await typeContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            HttpStatusCode status;
            var photoClient = typeContainerClient.GetBlobClient(blobPath);
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

        public async Task<bool> RemovePhotoAsync(string filename)
        {
            var name = filename.Substring(0, 40);
            var typeContainerClient = new BlobContainerClient(_connectionString, "photos");
            var blobs = typeContainerClient.GetBlobsAsync(prefix: name);
            await foreach (var blob in blobs)
            {
                var result = await typeContainerClient.DeleteBlobAsync(blob.Name, DeleteSnapshotsOption.IncludeSnapshots);
                if ((HttpStatusCode)result.Status != HttpStatusCode.Accepted)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> UploadPhotoStreamAsync(MemoryStream stream, string blobPath)
        {
            var typeContainerClient = new BlobContainerClient(_connectionString, "photos");
            await typeContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            HttpStatusCode status;
            var photoClient = typeContainerClient.GetBlobClient(blobPath);
            stream.Seek(0, SeekOrigin.Begin);

            var result = await photoClient.UploadAsync(stream);
            status = (HttpStatusCode)result.GetRawResponse().Status;

            return status == HttpStatusCode.Created ? true : false;
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
