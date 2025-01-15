
namespace Application.DTOs.Products
{
    public class GetProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
        public Guid[] ThumbnailsId { get; set; }
    }
}
