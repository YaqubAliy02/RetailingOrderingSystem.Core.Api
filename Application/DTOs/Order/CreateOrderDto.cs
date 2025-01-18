using Application.DTOs.OrderDetail;

namespace Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public Guid UserId { get; set; }
        public ICollection<CreateOrderDetailDto> OrderDetails { get; set; }
    }
}
