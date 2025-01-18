using Application.DTOs.OrderDetail;

namespace Application.DTOs.Order
{
    public class OrderDto
    {
        public Guid UserId { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}

