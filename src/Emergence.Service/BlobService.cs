using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Emergence.Service.Interfaces;
using ExifLib;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Emergence.Service
{
    public class BlobService : IBlobService
    {
        private readonly string _connectionString;
        private readonly IExifService _exifService;

        public BlobService(IConfiguration configuration, IExifService exifService)
        {
            _connectionString = configuration["AzureStorageConnectionString"];
            _exifService = exifService;
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

                if (status == HttpStatusCode.Created)
                {
                    var blobProperties = await SetBlobPropertiesAsync(stream, blobPath, userId, photo.ContentType);

                    return blobProperties;
                }
            }

            return null;
        }

        public async Task<bool> RemovePhotoAsync(string blobPath)
        {
            var typeContainerClient = new BlobContainerClient(_connectionString, "photos");
            var blobs = typeContainerClient.GetBlobsAsync(prefix: blobPath);
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

            var deleteResponse = await photoClient.DeleteIfExistsAsync();

            if (deleteResponse != null)
            {
                var result = await photoClient.UploadAsync(stream);
                status = (HttpStatusCode)result.GetRawResponse().Status;

                return status == HttpStatusCode.Created ? true : false;
            }
            return false;
        }

        public async Task<IBlobResult> SetBlobPropertiesAsync(Stream stream, string blobPath, string userId, string contentType)
        {
            var typeContainerClient = new BlobContainerClient(_connectionString, "photos");
            await typeContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            var photoClient = typeContainerClient.GetBlobClient(blobPath);
            stream.Seek(0, SeekOrigin.Begin);

            return await ReadAndSetBlobPropertiesAsync(photoClient, stream, userId, contentType);
        }

        public async Task<IBlobResult> SetBlobPropertiesAsync(IBlobResult blobProperties, string blobPath, string userId)
        {
            var typeContainerClient = new BlobContainerClient(_connectionString, "photos");
            await typeContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            var client = typeContainerClient.GetBlobClient(blobPath);
            var metadata = blobProperties.Metadata;
            return await SetBlobPropertiesAsync(client, metadata, userId, blobProperties.ContentType);
        }

        public async Task<IBlobResult> GetBlobPropertiesAsync(string blobPath)
        {
            var typeContainerClient = new BlobContainerClient(_connectionString, "photos");
            await typeContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            var client = typeContainerClient.GetBlobClient(blobPath);
            var metadataResult = await client.GetPropertiesAsync();

            return new BlobResult
            {
                Metadata = metadataResult.Value.Metadata,
                ContentType = metadataResult.Value.ContentType
            };
        }

        private async Task<IBlobResult> ReadAndSetBlobPropertiesAsync(BlobClient client, Stream stream, string userId, string contentType)
        {
            var reader = new ExifReader(stream);
            var metadata = _exifService.GetMetadata(reader);
            return await SetBlobPropertiesAsync(client, metadata, userId, contentType);
        }

        private async Task<IBlobResult> SetBlobPropertiesAsync(BlobClient client, IDictionary<string, string> metadata, string userId, string contentType)
        {
            if (!string.IsNullOrEmpty(userId) && !metadata.ContainsKey("UserId"))
            {
                metadata.Add("UserId", userId);
            }

            var result = await client.SetMetadataAsync(metadata);
            if (result.Value != null)
            {
                var metadataResult = (await client.GetPropertiesAsync()).Value;

                if (metadataResult != null)
                {
                    result = await client.SetHttpHeadersAsync(
                    new BlobHttpHeaders
                    {
                        ContentType = contentType,
                        ContentDisposition = metadataResult.ContentDisposition,
                        ContentHash = metadataResult.ContentHash,
                        ContentEncoding = metadataResult.ContentEncoding,
                        ContentLanguage = metadataResult.ContentLanguage
                    });

                    if (result != null)
                    {
                        return new BlobResult
                        {
                            Metadata = metadataResult.Metadata,
                            ContentType = metadataResult.ContentType
                        };
                    }
                }
            }
            return null;
        }
    }
}
