using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstraction
{
    public interface IRetailingOrderingSystemDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

