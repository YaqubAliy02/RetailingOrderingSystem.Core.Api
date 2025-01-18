using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRetailingOrderingSystemDbContext context;

        public ProductRepository(IRetailingOrderingSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await this.context.Products.AddAsync(product);
            int result = await this.context.SaveChangesAsync();

            return result > 0 ? product : null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var product = await this.context.Products.FindAsync(id);
            if (product is not null)
                this.context.Products.Remove(product);

            int result = await this.context.SaveChangesAsync();

            return result > 0 ? true : false;
        }

        public async Task<IQueryable<Product>> GetAllAsync(Expression<Func<Product, bool>> expression)
        {
            return this.context.Products
                .Where(expression)
                .Include(p => p.Category)
                .Include(p => p.OrderDetails)
                .Include(p => p.ProductThumbnails);
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await this.context.Products
                .Where(p => p.Id.Equals(id))
                .Include(p => p.Category)
                .Include(p => p.OrderDetails)
                .Include(p => p.ProductThumbnails)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            var existingProduct = this.context.Products.Find(product.Id);

            if (existingProduct is not null)
                this.context.Products.Update(product);

            var result = await this.context.SaveChangesAsync();

            return result > 0 ? product : null;
        }

        public async Task<bool> CheckStockAsync(Guid productId, int quantity)
        {
            var product = await this.context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            return product.Stock >= quantity;
        }

        public async Task DeductStockAsync(Guid productId, int quantity)
        {
            var product = await this.context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            if (product.Stock< quantity)
                throw new InvalidOperationException("Insufficient stock available.");

            product.Stock -= quantity;
            await this.context.SaveChangesAsync();
        }
    }
}
