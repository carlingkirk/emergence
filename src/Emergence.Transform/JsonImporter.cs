using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Emergence.Transform
{
    public interface IJsonImporter<T> : IImporter<T>
    {
        T ImportObjectAsync();
    }

    public class JsonImporter<T> : IJsonImporter<T>
    {
        private readonly string _filename;

        public JsonImporter(string filename)
        {
            _filename = filename;
        }

        public T ImportObjectAsync()
        {
            using (var file = File.OpenText(_filename))
            {
                var serializer = new JsonSerializer();
                return (T)serializer.Deserialize(file, typeof(T));
            }
        }

        public Task<T> ImportAsync() => throw new System.NotImplementedException();
        IAsyncEnumerable<T> IImporter<T>.Import() => throw new System.NotImplementedException();
    }
}
