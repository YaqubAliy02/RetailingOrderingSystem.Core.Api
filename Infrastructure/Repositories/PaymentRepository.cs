using Application.Abstraction;
using Application.DTOs.Order;
using Application.Repositories;
using AutoMapper;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IRetailingOrderingSystemDbContext context;
        private readonly IMapper mapper;
        public PaymentRepository(IRetailingOrderingSystemDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await this.context.Orders
                .Include(o => o.OrderDetails).FirstOrDefaultAsync(context => context.Id == orderId);
        }

        public async Task<OrderDto> UpdateOrderAsync(Order order)
        {
            if (order is not null)
                this.context.Orders.Update(order);

            var result = await this.context.SaveChangesAsync();
            var updatedOrder = this.mapper.Map<OrderDto>(order);

            return result > 0 ? updatedOrder : throw new Exception("Failed to update order.");
        }
    }
}
