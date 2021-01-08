using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Data.Extensions
{
    public static class RepositoryExtensions
    {
        public static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query, Func<IIncludable<T>, IIncludable> includes) where T : class
        {
            if (includes == null)
            {
                return query;
            }

            var includable = (Includable<T>)includes(new Includable<T>(query));
            return includable.Input;
        }

        public static IIncludable<TEntity, TProperty> Include<TEntity, TProperty>(this IIncludable<TEntity> includes,
            Expression<Func<TEntity, TProperty>> propertySelector) where TEntity : class
        {
            var result = ((Includable<TEntity>)includes).Input.Include(propertySelector);
            return new Includable<TEntity, TProperty>(result);
        }

        public static IIncludable<TEntity, TOtherProperty> ThenInclude<TEntity, TOtherProperty, TProperty>(this IIncludable<TEntity, TProperty> includes,
                Expression<Func<TProperty, TOtherProperty>> propertySelector) where TEntity : class
        {
            var result = ((Includable<TEntity, TProperty>)includes).IncludableInput.ThenInclude(propertySelector);
            return new Includable<TEntity, TOtherProperty>(result);
        }

        public static IIncludable<TEntity, TOtherProperty> ThenInclude<TEntity, TOtherProperty, TProperty>(this IIncludable<TEntity, IEnumerable<TProperty>> includes,
                Expression<Func<TProperty, TOtherProperty>> propertySelector) where TEntity : class
        {
            var result = ((Includable<TEntity, IEnumerable<TProperty>>)includes).IncludableInput.ThenInclude(propertySelector);
            return new Includable<TEntity, TOtherProperty>(result);
        }

        public static IQueryable<T> OrderByMultiple<T>(this IQueryable<T> query, Func<IOrderable<T>, IOrderable> orders) where T : class
        {
            if (orders == null)
            {
                return query;
            }

            var orderable = (Orderable<T>)orders(new Orderable<T>(query));

            return orderable.Input;
        }

        public static IOrderable<TEntity, TProperty> OrderBy<TEntity, TProperty>(this IOrderable<TEntity> orders,
            Expression<Func<TEntity, TProperty>> propertySelector) where TEntity : class
        {
            var result = ((Orderable<TEntity>)orders).Input.OrderBy(propertySelector);
            return new Orderable<TEntity, TProperty>(result);
        }

        public static IOrderable<TEntity, TOtherProperty> ThenBy<TEntity, TOtherProperty, TProperty>(this IOrderable<TEntity, TProperty> orders,
                Expression<Func<TEntity, TOtherProperty>> propertySelector) where TEntity : class
        {
            var result = ((Orderable<TEntity, TProperty>)orders).OrderableInput.ThenBy(propertySelector);
            return new Orderable<TEntity, TOtherProperty>(result);
        }

        public static IOrderable<TEntity, TOtherProperty> ThenBy<TEntity, TOtherProperty, TProperty>(this IOrderable<TEntity, IEnumerable<TProperty>> orders,
                Expression<Func<TEntity, TOtherProperty>> propertySelector) where TEntity : class
        {
            var result = ((Orderable<TEntity, IEnumerable<TProperty>>)orders).OrderableInput.ThenBy(propertySelector);
            return new Orderable<TEntity, TOtherProperty>(result);
        }

        public static IOrderable<TEntity, TProperty> OrderByDescending<TEntity, TProperty>(this IOrderable<TEntity> orders,
            Expression<Func<TEntity, TProperty>> propertySelector) where TEntity : class
        {
            var result = ((Orderable<TEntity>)orders).Input.OrderByDescending(propertySelector);
            return new Orderable<TEntity, TProperty>(result);
        }

        public static IOrderable<TEntity, TOtherProperty> ThenByDescending<TEntity, TOtherProperty, TProperty>(this IOrderable<TEntity, TProperty> orders,
                Expression<Func<TEntity, TOtherProperty>> propertySelector) where TEntity : class
        {
            var result = ((Orderable<TEntity, TProperty>)orders).OrderableInput.ThenByDescending(propertySelector);
            return new Orderable<TEntity, TOtherProperty>(result);
        }

        public static IOrderable<TEntity, TOtherProperty> ThenByDescending<TEntity, TOtherProperty, TProperty>(this IOrderable<TEntity, IEnumerable<TProperty>> orders,
                Expression<Func<TEntity, TOtherProperty>> propertySelector) where TEntity : class
        {
            var result = ((Orderable<TEntity, IEnumerable<TProperty>>)orders).OrderableInput.ThenByDescending(propertySelector);
            return new Orderable<TEntity, TOtherProperty>(result);
        }

        public static IQueryable<T> WithOrder<T>(this IQueryable<T> source, params Func<IOrderable<T>, IOrderable>[] orders) where T : class
        {
            if (orders != null)
            {
                foreach (var order in orders)
                {
                    source = source.OrderByMultiple(order);
                }
            }

            return source;
        }

        public static async IAsyncEnumerable<T> GetSomeAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate = null,
            int? skip = null, int? take = null, bool track = false) where T : class
        {
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            if (skip.HasValue)
            {
                source = source.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                source = source.Take(take.Value);
            }

            if (!track)
            {
                source = source.AsNoTracking();
            }

            await foreach (var entity in source.AsAsyncEnumerable())
            {
                yield return entity;
            }
        }

        public static IEnumerable<T> GetSome<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate = null,
            int? skip = null, int? take = null, bool track = false) where T : class
        {
            if (predicate != null)
            {
                source = source.Where(predicate);
            }

            if (skip.HasValue)
            {
                source = source.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                source = source.Take(take.Value);
            }

            if (!track)
            {
                source = source.AsNoTracking();
            }

            return source;
        }

        public static async Task<IEnumerable<T>> GetAllAsync<T>(this IQueryable<T> source) => await source.ToListAsync();
    }
}
