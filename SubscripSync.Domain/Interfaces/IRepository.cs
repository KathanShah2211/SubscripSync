using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SubscripSync.Domain.Common;

namespace SubscripSync.Domain.Interfaces
{
    public interface IRepository<T> where T : Entity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
