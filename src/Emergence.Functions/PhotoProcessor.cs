using System.IO;
using System.Threading.Tasks;
using Emergence.Functions.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace Emergence.Functions
{
    public class PhotoProcessor
    {
        private readonly IBlobService _blobService;
        private readonly IPhotoService _photoService;

        public PhotoProcessor(IBlobService blobService, IPhotoService photoService)
        {
            _blobService = blobService;
            _photoService = photoService;
        }

        [FunctionName("ProcessOriginalPhoto")]
        public async Task Run([BlobTrigger("photos/{name}/original{fileExt}", Connection = "AzureStorageConnectionString")]
            Stream photoStream, string name, string fileExt, ILogger log)
        {
            using (var image = Image.Load(photoStream))
            {
                // Remove EXIF data and save
                var exifProfile = image.Metadata.ExifProfile;

                if (exifProfile != null)
                {
                    RemoveTag(ExifTag.GPSLatitude, exifProfile);
                    RemoveTag(ExifTag.GPSLatitude, exifProfile);
                    RemoveTag(ExifTag.GPSLatitudeRef, exifProfile);
                    RemoveTag(ExifTag.GPSDestLatitude, exifProfile);
                    RemoveTag(ExifTag.GPSDestLatitudeRef, exifProfile);
                    RemoveTag(ExifTag.GPSLongitude, exifProfile);
                    RemoveTag(ExifTag.GPSLongitudeRef, exifProfile);
                    RemoveTag(ExifTag.GPSDestLongitude, exifProfile);
                    RemoveTag(ExifTag.GPSDestLongitudeRef, exifProfile);
                    RemoveTag(ExifTag.GPSAltitude, exifProfile);
                    RemoveTag(ExifTag.GPSAltitudeRef, exifProfile);
                }

                var originalName = "original" + fileExt;
                // Large
                var largeResult = await ProcessPhoto(image, name, originalName, ImageSize.Large);

                // Medium
                var mediumResult = await ProcessPhoto(image, name, originalName, ImageSize.Medium);

                // Thumb
                var thumbResult = await ProcessPhoto(image, name, originalName, ImageSize.Thumb);

                if (largeResult && mediumResult && thumbResult)
                {
                    log.LogInformation($"ProcessOriginalPhoto Processed blob\n Name:{name} \n File: original{fileExt} \n Size: {photoStream.Length} Bytes");
                }
            }
        }

        private async Task<bool> ProcessPhoto(Image image, string name, string originalName, ImageSize imageSize)
        {
            using (var memoryStream = new MemoryStream())
            {
                var processedImage = await _photoService.ProcessPhotoAsync(memoryStream, image, imageSize);
                var blobPath = $"{name}/{imageSize.ToString().ToLowerInvariant()}.png";

                await _blobService.UploadPhotoStreamAsync(memoryStream, blobPath);

                var blobProperties = await _blobService.GetBlobPropertiesAsync($"{name}/{originalName}");
                blobProperties.Metadata.TryGetValue("UserId", out var userId);

                var result = await _blobService.SetBlobPropertiesAsync(blobProperties, blobPath, userId);
                if (result != null)
                {
                    return true;
                }
            }
            return false;
        }

        private static void RemoveTag<T>(ExifTag<T> exifTag, ExifProfile exifProfile)
        {
            if (exifProfile.GetValue<T>(exifTag) != null)
            {
                exifProfile.RemoveValue(exifTag);
            }
        }
    }
}
