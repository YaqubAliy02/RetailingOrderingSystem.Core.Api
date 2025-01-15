using Domain.Models;

namespace Application.Repositories
{
    public interface IProductThumbnailRepository
    {
        Task<ProductThumbnail> AddProductThumbnailAsync(Guid productId, Stream photoStream, string fileName, string contentType);
        Task<ProductThumbnail> AddForUpdateProductThumbnailAsync(Stream photoStream, string fileName, string contentType);
        Task<bool> DeleteAsync(Guid id);

    }
}
