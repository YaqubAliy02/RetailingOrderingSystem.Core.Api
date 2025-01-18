using Application.DTOs.OrderDetail;
using Domain.Models;

namespace Application.DTOs.Order
{
    public class OrderDto
    {
        public Guid UserId { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}

