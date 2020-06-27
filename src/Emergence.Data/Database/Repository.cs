using Emergence.Data.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Emergence.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IKeyable
    {
        private EmergenceDbContext _context;

        public Repository(EmergenceDbContext context)
        {
            _context = context;
        }

        public async IAsyncEnumerable<T> GetSome(Expression<Func<T, bool>> predicate, bool track = false)
        {
            var entities = _context.Set<T>().Where(predicate);

            if (!track)
            {
                entities = entities.AsNoTracking();
            };

            await foreach (var entity in entities.AsAsyncEnumerable())
            {
                yield return entity;
            }
        }

        public async Task<T> Get(Expression<Func<T, bool>> predicate, bool track = false)
        {
            var entities = _context.Set<T>().Where(predicate);

            if (!track)
            {
                entities = entities.AsNoTracking();
            };

            return await entities.FirstOrDefaultAsync();
        }

        public async Task<T> AddOrUpdate(Expression<Func<T, bool>> key, T entity)
        {
            T dbEntity;

            if (_context.Set<T>().Any(key))
            {
                dbEntity = _context.Set<T>().Update(entity).Entity;
            }
            else
            {
                dbEntity =  (await _context.Set<T>().AddAsync(entity)).Entity;
            }

            return dbEntity;
        }

        public async Task<T> AddOrUpdate(object key, T entity)
        {
            return await AddOrUpdate(e => e.Key == key, entity);
        }

        public async Task AddSome(IEnumerable<T> source)
        {
            await _context.Set<T>().AddRangeAsync(source);
            await _context.SaveChangesAsync();
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSome(IEnumerable<T> source)
        {
            _context.Set<T>().UpdateRange(source);
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
