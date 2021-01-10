using System.Collections.Generic;

namespace Emergence.Data.Shared
{
    public class FindResult<T> where T : class
    {
        public long Count { get; set; }
        public IEnumerable<T> Results { get; set; }
    }
}
