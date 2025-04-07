using Abstraction.Interfaces.Repositories;
using Core.Contexts;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        public readonly WeatherContext _context;

        public Repository(WeatherContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var result = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
            return result;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = await _context.Set<T>().ToListAsync();
            return result;
        }
    }
}
