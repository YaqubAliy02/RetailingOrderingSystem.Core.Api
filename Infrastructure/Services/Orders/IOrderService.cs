using Application.DTOs.Order;

namespace Infrastructure.Services.Orders
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
