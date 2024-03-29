using System.Collections.Generic;

namespace Emergence.Functions.Services
{
    public interface IBlobResult
    {
        IDictionary<string, string> Metadata { get; set; }
        string ContentType { get; set; }
    }

    public class BlobResult : IBlobResult
    {
        public IDictionary<string, string> Metadata { get; set; }
        public string ContentType { get; set; }
    }
}
