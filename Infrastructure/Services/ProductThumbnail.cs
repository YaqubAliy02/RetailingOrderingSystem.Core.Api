using System.Linq.Expressions;
using Application.Abstraction;
using Application.Repositories;
using Domain.Models;
using Infrastructure.External.AWSS3;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services
{
    public class ProductThumbnailRepository : IProductThumbnailRepository
    {
        private readonly IAWSStorage awsStorage;
        private readonly IProductRepository productRepository;
        private readonly IRetailingOrderingSystemDbContext context;

        public ProductThumbnailRepository(
            IAWSStorage awsStorage,
            IProductRepository productRepository,
            IRetailingOrderingSystemDbContext context)
        {
            this.awsStorage = awsStorage;
            this.productRepository = productRepository;
            this.context = context;
        }

        public async Task<ProductThumbnail> AddProductThumbnailAsync(Guid productId, Stream photoStream, string fileName, string contentType)
        {
            var product = await this.productRepository.GetByIdAsync(productId);

            if (product is null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
            }

            string s3Uri = await this.awsStorage.UploadFileAsync(photoStream, fileName, contentType);

            var productThumbnail = new ProductThumbnail
            {
                Id = Guid.NewGuid(),
                ProductId = productId,
                Awss3Uri = s3Uri,
                FileName = Path.GetFileName(fileName),
                Size = photoStream.Length,
                ContentType = contentType,
                UploadedDate = DateTimeOffset.UtcNow
            };

            await AddAsync(productThumbnail);

            return productThumbnail;
        }

        public async Task<ProductThumbnail> AddForUpdateProductThumbnailAsync(Stream photoStream, string fileName, string contentType)
        {
            string s3Uri = await this.awsStorage.UploadFileAsync(photoStream, fileName, contentType);

            var productThumbnail = new ProductThumbnail
            {
                Id = Guid.NewGuid(),
                Awss3Uri = s3Uri,
                FileName = Path.GetFileName(fileName),
                Size = photoStream.Length,
                ContentType = contentType,
                UploadedDate = DateTimeOffset.UtcNow
            };

            await AddAsync(productThumbnail);

            return productThumbnail;
        }

        public async Task<ProductThumbnail> AddAsync(ProductThumbnail productThumbnail)
        {
            await this.context.ProductThumbnails.AddAsync(productThumbnail);
            int result = await this.context.SaveChangesAsync();

            return result > 0 ? productThumbnail : null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var productThumbnail = await this.context.ProductThumbnails.FindAsync(id);

            if (productThumbnail is not null)
            {
                await this.awsStorage.DeleteFileAsync(productThumbnail.FileName);

                this.context.ProductThumbnails.Remove(productThumbnail);
            }

            int result = await this.context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<IQueryable<ProductThumbnail>> GetAsync(Expression<Func<ProductThumbnail, bool>> expression)
        {
            return this.context.ProductThumbnails
                .Where(expression)
                .AsQueryable();
        }

        public async Task<ProductThumbnail> GetByIdAsync(Guid id)
        {
            return await this.context.ProductThumbnails
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<ProductThumbnail> UpdateAsync(ProductThumbnail updateProductThumbnail)
        {
            var existingProductThumbnail = await this.GetByIdAsync(updateProductThumbnail.Id);

            if (existingProductThumbnail is not null)
            {
                this.context.ProductThumbnails.Update(updateProductThumbnail);
                int result = await this.context.SaveChangesAsync();

                if (result > 0) return updateProductThumbnail;
            }

            return null;
        }
    }
}
