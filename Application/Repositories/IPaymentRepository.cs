using Application.DTOs.Order;
using Domain.Models;

namespace Application.Repositories
{
    public interface IPaymentRepository
    {
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<OrderDto> UpdateOrderAsync(Order order);
    }
}
