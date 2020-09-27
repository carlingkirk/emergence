using System;
using System.Collections.Generic;
using System.Linq;

namespace Emergence.Service.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
            => items.GroupBy(property).Select(x => x.First());
        public static IQueryable<T> DistinctBy<T, TKey>(this IQueryable<T> items, Func<T, TKey> property)
            => items.GroupBy(property).Select(x => x.First()).AsQueryable();
    }
}
