using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Emergence.Data.Extensions
{
    public static class RepositoryExtensions
    {
        public static IQueryable<T> Include<T>(this IQueryable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query;
        }

        public static IAsyncEnumerable<T> Include<T>(this IAsyncEnumerable<T> query, params Expression<Func<T, object>>[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query;
        }
    }
}
