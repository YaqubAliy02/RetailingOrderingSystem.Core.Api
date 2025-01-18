using Domain.Models;

namespace Application.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<bool> CheckStockAsync(Guid productId, int quantity);
        Task DeductStockAsync(Guid productId, int quantity);
    }
}
