using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Emergence.Data
{
    public interface IRepository<T>
    {
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool track = false);
        Task<T> GetAsync(object key, bool track = false);
        IAsyncEnumerable<T> GetSomeAsync(Expression<Func<T, bool>> predicate, bool track = false);
        Task<T> AddOrUpdateAsync(T entity);
        Task<T> AddOrUpdateAsync(object key, T entity);
        Task AddSomeAsync(IEnumerable<T> source);
        Task AddAsync(T entity);
        Task UpdateSomeAsync(IEnumerable<T> source);
        Task UpdateAsync(T entity);
    }
}
