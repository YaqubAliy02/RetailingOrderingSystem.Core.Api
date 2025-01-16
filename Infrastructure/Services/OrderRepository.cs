using Application.Abstraction;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IRetailingOrderingSystemDbContext context;

        public OrderRepository(IRetailingOrderingSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await context.Orders.FindAsync(orderId);
            if (order == null) throw new KeyNotFoundException("Order not found.");

            order.PaymentStatus = status;
            await context.SaveChangesAsync();
        }
    }
}
