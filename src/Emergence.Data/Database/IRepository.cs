using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Emergence.Data
{
    public interface IRepository<T>
    {
        Task<T> Get(Expression<Func<T, bool>> predicate, bool track = false);
        Task<T> Get(object key, bool track = false);
        IAsyncEnumerable<T> GetSome(Expression<Func<T, bool>> predicate, bool track = false);
        Task<T> AddOrUpdate(Expression<Func<T, bool>> key, T entity);
        Task<T> AddOrUpdate(object key, T entity);
        Task AddSome(IEnumerable<T> source);
        Task Add(T entity);
        Task UpdateSome(IEnumerable<T> source);
        Task Update(T entity);
    }
}
