using Domain.Models;

namespace Application.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task UpdateOrderStatusAsync(int orderId, string status);
    }
}
