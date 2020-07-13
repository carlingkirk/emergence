using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared;

namespace Emergence.Data
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool track = false);
        Task<T> GetWithIncludesAsync(Expression<Func<T, bool>> predicate, bool track = false, params Func<IIncludable<T>, IIncludable>[] includes);
        IAsyncEnumerable<T> GetSomeAsync(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null, bool track = false);
        IAsyncEnumerable<T> GetSomeWithIncludesAsync(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null, bool track = false, params Func<IIncludable<T>, IIncludable>[] includes);
        Task<T> AddOrUpdateAsync(Expression<Func<T, bool>> key, T entity);
        Task AddSomeAsync(IEnumerable<T> source);
        Task AddAsync(T entity);
        Task UpdateSomeAsync(IEnumerable<T> source);
        Task UpdateAsync(T entity);
    }
}
