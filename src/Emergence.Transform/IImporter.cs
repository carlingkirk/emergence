using System.Collections.Generic;

namespace Emergence.Transform
{
    public interface IImporter<T>
    {
        public IAsyncEnumerable<T> Import();
    }
}
