using static System.Net.Mime.MediaTypeNames;

namespace Emergence.Service.Interfaces
{
    public interface IImageService
    {
        Image ResizePhoto(Image image, int maxDimension);
    }
}
