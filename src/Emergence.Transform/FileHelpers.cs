using System.IO;

namespace Emergence.Transform
{
    public static class FileHelpers
    {
        public static string GetDatafileName(string filename, string dataDirectory)
        {
            return Path.Combine(dataDirectory, filename);
        }
    }
}
