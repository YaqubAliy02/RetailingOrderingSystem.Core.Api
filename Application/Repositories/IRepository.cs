using System.Linq.Expressions;

namespace Application.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<IQueryable<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}

