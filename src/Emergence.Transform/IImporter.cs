using System.Collections.Generic;
using System.Threading.Tasks;

namespace Emergence.Transform
{
    public interface IImporter<T>
    {
        public IAsyncEnumerable<T> Import();
    }
}
