using System.Collections.Generic;
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

        public async Task<IBlobResult> UploadPhotoAsync(IFormFile photo, string path, string name)
        {
            var photoContainerClient = new BlobContainerClient(_connectionString, path);

            await photoContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            using (var stream = photo.OpenReadStream())
            {
                using (var exifReader = new ExifReader(stream))
                {
                    var metadata = GetMetadata(exifReader);
                    var result = await photoContainerClient.UploadBlobAsync(name, stream);
                    var blobClient = photoContainerClient.GetBlobClient(name);

                    await blobClient.SetMetadataAsync(metadata);
                    var metadataResult = blobClient.GetProperties().Value;
                    return new BlobResult
                    {
                        Metadata = metadataResult.Metadata,
                        ContentType = metadataResult.ContentType
                    };
                }
            }
        }

        public async Task<IEnumerable<IBlobResult>> UploadPhotosAsync(IEnumerable<IFormFile> photos, string path, string name)
        {
            var results = new List<BlobResult>();
            var photoContainerClient = new BlobContainerClient(_connectionString, path);
            await photoContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

            foreach (var photo in photos)
            {
                using (var stream = photo.OpenReadStream())
                {
                    using (var exifReader = new ExifReader(stream))
                    {
                        var metadata = GetMetadata(exifReader);
                        var result = await photoContainerClient.UploadBlobAsync(name, stream);
                        var blobClient = photoContainerClient.GetBlobClient(name);
                        await blobClient.SetMetadataAsync(metadata);
                        var metadataResult = blobClient.GetProperties().Value;
                        results.Add(new BlobResult
                        {
                            Metadata = metadataResult.Metadata,
                            ContentType = metadataResult.ContentType
                        });
                    }
                }
            }
            return results;
        }

        private IDictionary<string, string> GetMetadata(ExifReader reader)
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
