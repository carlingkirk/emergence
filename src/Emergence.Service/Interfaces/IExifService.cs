using System.Collections.Generic;
using ExifLib;

namespace Emergence.Service.Interfaces
{
    public interface IExifService
    {
        Dictionary<string, string> GetMetadata(ExifReader exifReader);
    }
}
