using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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

        public async IAsyncEnumerable<T> GetSomeAsync(Expression<Func<T, bool>> predicate, int? skip = null, int? take = null, bool track = false, params string[] includes)
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

            foreach (var include in includes)
            {
                entities = entities.Include(include);
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

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, bool track = false, params string[] includes)
        {
            var entities = _context.Set<T>().Where(predicate);

            foreach (var include in includes)
            {
                entities = entities.Include(include);
            }

            if (!track)
            {
                entities = entities.AsNoTracking();
            };

            return await entities.FirstOrDefaultAsync();
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

        public async Task AddSomeAsync(IEnumerable<T> source)
        {
            await _context.Set<T>().AddRangeAsync(source);
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSomeAsync(IEnumerable<T> source)
        {
            _context.Set<T>().UpdateRange(source);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
