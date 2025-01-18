using Application.DTOs.Order;
using Domain.Models;

namespace Infrastructure.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(Guid id);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto orderDto);
        Task<OrderDto> UpdateOrderStatusAsync(Guid id, string status);
        Task<bool> DeleteOrderAsync(Guid id);
    }
}
