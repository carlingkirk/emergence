using Emergence.Data.Shared;
using Microsoft.EntityFrameworkCore.Query;

namespace Emergence.Data
{
    internal class Includable<TEntity, TProperty> : Includable<TEntity>, IIncludable<TEntity, TProperty> where TEntity : class
    {
        internal IIncludableQueryable<TEntity, TProperty> IncludableInput { get; }

        internal Includable(IIncludableQueryable<TEntity, TProperty> queryable) : base(queryable)
        {
            IncludableInput = queryable;
        }
    }
}
