using Application.DTOs.Order;
using Infrastructure.Services.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RetailingOrderingSystem.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ApiControllerBase
    {
        private readonly IOrderService orderService;

        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            var orders = await this.orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            try
            {
                var order = await this.orderService.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatusAsync(Guid id, [FromBody] string status)
        {
            try
            {
                await this.orderService.UpdateOrderStatusAsync(id, status);
                return Ok(new { message = "Order status updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<OrderDto>> CreateOrderAsync([FromBody] CreateOrderDto orderDto)
        {
            try
            {
                var createdOrder = await this.orderService.CreateOrderAsync(orderDto);
                return CreatedAtAction(nameof(GetOrderByIdAsync), new { id = createdOrder.UserId }, createdOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            try
            {
                await this.orderService.DeleteOrderAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
