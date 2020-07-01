using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Emergence.Data.Shared.Stores;
using Microsoft.EntityFrameworkCore;

namespace Emergence.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IKeyable
    {
        private readonly EmergenceDbContext _context;

        public Repository(EmergenceDbContext context)
        {
            _context = context;
        }

        public async IAsyncEnumerable<T> GetSomeAsync(Expression<Func<T, bool>> predicate, bool track = false)
        {
            var entities = _context.Set<T>().Where(predicate);
            var items = _context.Inventories.Where(i => i.Id == 0).ToList();

            if (!track)
            {
                entities = entities.AsNoTracking();
            };

            await foreach (var entity in entities.AsAsyncEnumerable())
            {
                yield return entity;
            }
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

        public async Task<T> GetAsync(object key, bool track = false) => await GetAsync(e => e.Key == key, track);

        public async Task<T> AddOrUpdateAsync(T entity)
        {
            T dbEntity;

            if (_context.Set<T>().AsEnumerable().Any(e => e.Key == entity.Key))
            {
                dbEntity = _context.Set<T>().Update(entity).Entity;
            }
            else
            {
                dbEntity = (await _context.Set<T>().AddAsync(entity)).Entity;
            }

            return dbEntity;
        }

        public async Task<T> AddOrUpdateAsync(object key, T entity) => await AddOrUpdateAsync(entity);

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
