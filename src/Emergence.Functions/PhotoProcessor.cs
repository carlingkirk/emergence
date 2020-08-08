using System.IO;
using System.Threading.Tasks;
using Emergence.Service.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;

namespace Emergence.Functions
{
    public class PhotoProcessor
    {
        private readonly IBlobService _blobService;
        public PhotoProcessor(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [FunctionName("ProcessOriginalPhoto")]
        public async Task Run([BlobTrigger("photos/{name}/original{fileExt}", Connection = "AzureStorageConnectionString")]
            Stream photoStream, string name, string fileExt, ILogger log)
        {
            using (var image = Image.Load(photoStream))
            {
                // Remove EXIF data and save
                var exifProfile = image.Metadata.ExifProfile;

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

                using (var memoryStream = new MemoryStream())
                {
                    image.SaveAsPng(memoryStream);
                    var blobPath = $"{name}/full.png";
                    await _blobService.UploadPhotoStreamAsync(memoryStream, blobPath);
                }

                // Resize if needed
                var maxDimension = 1600;
                if (image.Width > maxDimension || image.Height > maxDimension)
                {
                    using (var mediumImage = ResizePhoto(image, maxDimension))
                    using (var memoryStream = new MemoryStream())
                    {
                        mediumImage.SaveAsPng(memoryStream);
                        var blobPath = $"{name}/medium.png";
                        await _blobService.UploadPhotoStreamAsync(memoryStream, blobPath);
                    }
                }

                // Save thumbnail
                using (var thumbImage = ResizePhoto(image, 150))
                using (var memoryStream = new MemoryStream())
                {
                    thumbImage.SaveAsPng(memoryStream);
                    var blobPath = $"{name}/thumb.png";
                    await _blobService.UploadPhotoStreamAsync(memoryStream, blobPath);
                }
            }

            log.LogInformation($"ProcessActivityPhoto Processed blob\n Name:{name} \n File: original{fileExt} \n Size: {photoStream.Length} Bytes");
        }

        private static void RemoveTag<T>(ExifTag<T> exifTag, ExifProfile exifProfile)
        {
            if (exifProfile.GetValue<T>(exifTag) != null)
            {
                exifProfile.RemoveValue(exifTag);
            }
        }

        private static Image ResizePhoto(Image image, int maxDimension)
        {
            var aspectRatio = image.Width / image.Height;
            var newWidth = aspectRatio > 1 ? maxDimension : maxDimension * aspectRatio;
            var newHeight = aspectRatio < 1 ? maxDimension : maxDimension * aspectRatio;
            return image.Clone(i => i.Resize(newWidth, newHeight));
        }
    }
}
