using ChallengeAPISegurarse.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAPISegurarse.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<Cliente> GetByDniAsync(int dni);


    }
}
