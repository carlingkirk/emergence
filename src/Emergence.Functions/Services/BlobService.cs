using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Emergence.Functions.Services
{
    public class BlobService : IBlobService
    {
        private readonly string _connectionString;
        private readonly ILogger<BlobService> _logger;

        public BlobService(IConfiguration configuration,  ILogger<BlobService> logger)
        {
            _connectionString = configuration["AzureStorageConnectionString"];
            _logger = logger;
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

        public async Task<IBlobResult> SetBlobPropertiesAsync(IBlobResult blobProperties, string blobPath, string userId)
        {
            var typeContainerClient = new BlobContainerClient(_connectionString, "photos");
            await typeContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            var client = typeContainerClient.GetBlobClient(blobPath);
            var metadata = blobProperties.Metadata;
            return await SetBlobPropertiesAsync(client, metadata, userId, blobProperties.ContentType);
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
