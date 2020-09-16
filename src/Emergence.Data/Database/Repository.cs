using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Extensions;
using Emergence.Data.Shared;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EmergenceDbContext _context;

        public Repository(EmergenceDbContext context)
        {
            _context = context;
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            var entities = _context.Set<T>().Where(predicate);
            return entities;
        }

        public IQueryable<T> WhereWithIncludes(Expression<Func<T, bool>> predicate, params Func<IIncludable<T>, IIncludable>[] includes)
        {
            var entities = _context.Set<T>().Where(predicate);
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    entities = entities.IncludeMultiple(include);
                }
            }
            return entities;
        }


        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool track = false)
        {
            var entities = _context.Set<T>().Where(predicate);

            if (!track)
            {
                entities = entities.AsNoTracking();
            };

            return await entities.FirstOrDefaultAsync();
        }

        public async Task<T> GetWithIncludesAsync(Expression<Func<T, bool>> predicate, bool track = false, params Func<IIncludable<T>, IIncludable>[] includes)
        {
            var entities = _context.Set<T>().Where(predicate);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    entities = entities.IncludeMultiple(include);
                }
            }

            if (!track)
            {
                entities = entities.AsNoTracking();
            };

            return await entities.FirstOrDefaultAsync();
        }

        public async IAsyncEnumerable<T> GetSomeAsync(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null, bool track = false)
        {
            var entities = _context.Set<T>().Where(predicate);

            if (skip.HasValue)
            {
                entities = entities.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                entities = entities.Take(take.Value);
            }

            if (!track)
            {
                entities = entities.AsNoTracking();
            };

            await foreach (var entity in entities.AsAsyncEnumerable())
            {
                yield return entity;
            }
        }

        public async Task<T> AddOrUpdateAsync(Expression<Func<T, bool>> key, T entity)
        {
            T dbEntity;

            if (_context.Set<T>().AsQueryable<T>().Any(key))
            {
                dbEntity = _context.Set<T>().Update(entity).Entity;
            }
            else
            {
                dbEntity = (await _context.Set<T>().AddAsync(entity)).Entity;
            }

            await _context.SaveChangesAsync();

            return dbEntity;
        }

        public async Task<IEnumerable<T>> AddSomeAsync(IEnumerable<T> source)
        {
            var entities = source.ToList();
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return entities;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> UpdateSomeAsync(IEnumerable<T> source)
        {
            var entities = source.ToList();
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return entities;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveManyAsync(IEnumerable<T> source)
        {
            _context.Set<T>().RemoveRange(source);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
