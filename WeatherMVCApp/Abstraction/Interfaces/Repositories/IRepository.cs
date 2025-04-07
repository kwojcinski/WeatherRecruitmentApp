using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Interfaces.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<int> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
    }
}
