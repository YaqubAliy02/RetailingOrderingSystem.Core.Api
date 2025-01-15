using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class ProductThumbnail
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string Awss3Uri { get; set; }
        public Guid ProductId { get; set; }
        [JsonIgnore]
        public  Product Product { get; set; }
        public DateTimeOffset UploadedDate { get; set; }
    }
}
