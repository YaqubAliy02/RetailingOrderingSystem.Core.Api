using Domain.Models;

namespace Application.Repositories
{
    public interface IOrderRepository
    {
        Task<IQueryable<Order>> GetAllOrdersWithDetailsAsync();
        Task<Order> GetOrderWithDetailsByIdAsync(Guid id);
    }

}
