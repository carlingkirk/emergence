using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Emergence.Functions.Services
{
    public class PhotoService : IPhotoService
    {
        public async Task<Image> ProcessPhotoAsync(Stream stream, Image image, ImageSize imageSize)
        {
            image = OrientPhoto(image);
            image = ResizePhoto(image, (int)imageSize);
            image = await SaveAsPngAsync(stream, image);

            return image;
        }

        private Image OrientPhoto(Image image)
        {
            image.Mutate(i => i.AutoOrient());
            return image;
        }

        private Image ResizePhoto(Image image, int maxDimension)
        {
            if (image.Width > maxDimension || image.Height > maxDimension)
            {
                var aspectRatio = image.Width / (double)image.Height;
                var newWidth = (int)Math.Floor(aspectRatio > 1 ? maxDimension : maxDimension / aspectRatio);
                var newHeight = (int)Math.Floor(aspectRatio < 1 ? maxDimension : maxDimension / aspectRatio);
                return image.Clone(i => i.Resize(newWidth, newHeight));
            }
            return image;
        }

        private async Task<Image> SaveAsPngAsync(Stream stream, Image image)
        {
            var pngEncoder = new PngEncoder
            {
                CompressionLevel = PngCompressionLevel.BestCompression,
                BitDepth = PngBitDepth.Bit8,
                IgnoreMetadata = true,
                FilterMethod = PngFilterMethod.Adaptive
            };

            await image.SaveAsPngAsync(stream, pngEncoder);

            return image;
        }
    }
}
