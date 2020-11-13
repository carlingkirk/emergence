using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;

namespace Emergence.Functions.Services
{
    public interface IPhotoService
    {
        Task<Image> ProcessPhotoAsync(Stream stream, Image image, ImageSize imageSize);
    }
}
