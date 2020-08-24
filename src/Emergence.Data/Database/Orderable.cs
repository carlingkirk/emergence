using System.Linq;
using Emergence.Data.Shared;

namespace Emergence.Data
{
    internal class Orderable<TEntity, TProperty> : Orderable<TEntity>, IOrderable<TEntity, TProperty> where TEntity : class
    {
        internal IOrderedQueryable<TEntity> OrderableInput { get; }

        internal Orderable(IOrderedQueryable<TEntity> queryable) : base(queryable)
        {
            OrderableInput = queryable;
        }
    }
}
