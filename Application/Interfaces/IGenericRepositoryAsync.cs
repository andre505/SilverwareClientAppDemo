using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IGenericRepositoryAsync<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<T> AddAsync(T entity, string userId = null);
        Task UpdateAsync(T entity, string userId = null);
        Task DeleteAsync(T entity, string userId = null);
    }
}
