using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repository;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IRetailingOrderingSystemDbContext context;

        public CategoryRepository(IRetailingOrderingSystemDbContext context)
        {
            this.context = context;
        }

        public async Task<Category> AddAsync(Category category)
        {
            await this.context.Categories.AddAsync(category);
            int result = await this.context.SaveChangesAsync();

            if (result > 0) return category;

            return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var category = await this.context.Categories.FindAsync(id);

            if (category is not null)
            {
                this.context.Categories.Remove(category);
            }

            int result = await this.context.SaveChangesAsync();

            if (result > 0) return true;

            return false;
        }

        public async Task<IQueryable<Category>> GetAllAsync(Expression<Func<Category, bool>> expression)
        {
            return this.context.Categories.Where(expression)
                .Include(category => category.Products);
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            return this.context.Categories
                .Where(c => c.Id.Equals(id))
                .Include(category => category.Products)
                .FirstOrDefault();
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var existingCategory = await GetByIdAsync(category.Id);

            if (existingCategory is not null)
            {
                existingCategory.Name = category.Name;
                existingCategory.Description = category.Description;

                foreach (var product in existingCategory.Products)
                {
                    var existingProducts = this.context.Products.FindAsync(product.Id);

                    if (existingCategory is not null)
                    {
                        existingCategory.Products.Add(product);
                    }
                }

                var result = await this.context.SaveChangesAsync();

                if (result > 0) return category;
            }

            return null;
        }
    }
}
