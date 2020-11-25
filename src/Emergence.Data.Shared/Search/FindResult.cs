using System.Collections.Generic;

namespace Emergence.Data.Shared
{
    public class FindResult<T> where T : class
    {
        public int Count { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
