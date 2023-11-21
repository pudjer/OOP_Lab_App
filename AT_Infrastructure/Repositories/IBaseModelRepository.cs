using AT_Domain.Models;

namespace AT_Infrastructure.Repositories
{
    public interface IBaseModelRepository<T> where T : BaseModel
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<T?> GetAsync(Guid id);
        public Task<T> AddAsync(T entity);
        public Task<T?> UpdateAsync(T entity);
        public Task DeleteAsync(T entity);
    }
}
