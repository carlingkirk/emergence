using System;
using System.Linq;

namespace Emergence.Data.Shared
{
    public interface IIncludable { }

    public interface IIncludable<out TEntity> : IIncludable { }

    public interface IIncludable<out TEntity, out TProperty> : IIncludable<TEntity> { }

    public class Includable<TEntity> : IIncludable<TEntity> where TEntity : class
    {
        public IQueryable<TEntity> Input { get; }

        public Includable(IQueryable<TEntity> queryable)
        {
            Input = queryable ?? throw new ArgumentNullException(nameof(queryable));
        }
    }
}
