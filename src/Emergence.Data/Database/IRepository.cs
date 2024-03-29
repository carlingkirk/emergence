using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared;

namespace Emergence.Data
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate, bool track = false);
        IQueryable<T> WhereWithIncludes(Expression<Func<T, bool>> predicate, bool track = false, params Func<IIncludable<T>, IIncludable>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool track = false);
        Task<T> GetWithIncludesAsync(Expression<Func<T, bool>> predicate, bool track = false, params Func<IIncludable<T>, IIncludable>[] includes);
        IAsyncEnumerable<T> GetSomeAsync(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null, bool track = false);
        IQueryable<T> GetSome(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null, bool track = false);
        Task<T> AddOrUpdateAsync(Expression<Func<T, bool>> key, T entity);
        Task<IEnumerable<T>> AddSomeAsync(IEnumerable<T> source);
        Task AddAsync(T entity);
        Task<IEnumerable<T>> UpdateSomeAsync(IEnumerable<T> source);
        Task<T> UpdateAsync(T entity);
        Task<bool> RemoveAsync(T entity);
        Task<bool> RemoveManyAsync(IEnumerable<T> source);
    }
}
