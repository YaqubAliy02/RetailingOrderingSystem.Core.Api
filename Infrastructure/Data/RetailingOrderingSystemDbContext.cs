using Application.Abstraction;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data
{
    public class RetailingOrderingSystemDbContext : DbContext, IRetailingOrderingSystemDbContext
    {
        private readonly IConfiguration configuration;
        public RetailingOrderingSystemDbContext(DbContextOptions<RetailingOrderingSystemDbContext> options, 
            IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

       public DbSet<User> Users { get; set; }
       public DbSet<Product> Products { get; set; }
       public DbSet<Category> Categories { get; set; }
       public DbSet<Order> Orders { get; set; }
       public DbSet<OrderDetail> OrderDetails { get; set; }
       public DbSet<ProductThumbnail> ProductThumbnails { get; set; }
       public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(option => 
            option.Price).HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Order>().Property
                (option => option.TotalAmount).HasColumnType("decimal(10,2)");
            
            modelBuilder.Entity<OrderDetail>().Property
                (options => options.Price).HasColumnType("decimal(10,2)");

            modelBuilder.Entity<User>().HasIndex(option => option.Email).IsUnique();
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Role) 
                .HasConversion<string>();
        }
    }
}
