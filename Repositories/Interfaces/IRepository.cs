using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baynatna.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync();
        Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize);
        Task SaveChangesAsync();
    }
} 