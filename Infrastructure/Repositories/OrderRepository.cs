using Application.Abstraction;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IRetailingOrderingSystemDbContext context;

        public OrderRepository(IRetailingOrderingSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<IQueryable<Order>> GetAllOrdersWithDetailsAsync()
        {
            return this.context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                .ThenInclude(o => o.Product);
        }

        public async Task<Order> GetOrderWithDetailsByIdAsync(Guid id)
        {
            return await this.context.Orders
                 .Include(o => o.User)
                 .Include(o => o.OrderDetails)
                     .ThenInclude(oi => oi.Product)
                 .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}
