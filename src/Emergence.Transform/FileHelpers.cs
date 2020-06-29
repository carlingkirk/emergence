using System.IO;

namespace Emergence.Transform
{
    public static class FileHelpers
    {
        public static string GetDatafileName(string filename, string dataDirectory) => Path.Combine(dataDirectory, filename);
    }
}
