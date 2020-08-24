using System;
using System.Linq;

namespace Emergence.Data.Shared
{
    public interface IOrderable { }

    public interface IOrderable<out TEntity> : IOrderable { }

    public interface IOrderable<out TEntity, out TProperty> : IOrderable<TEntity> { }

    public class Orderable<TEntity> : IOrderable<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Input { get; }

        public Orderable(IQueryable<TEntity> queryable)
        {
            Input = queryable ?? throw new ArgumentNullException(nameof(queryable));
        }
    }
}
