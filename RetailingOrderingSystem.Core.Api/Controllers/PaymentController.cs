using Infrastructure.Services.Payments;
using Microsoft.AspNetCore.Mvc;

namespace RetailingOrderingSystem.Core.Api.Controllers
{
    public class PaymentController : ApiControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ProcessPaymentAsync(Guid orderId)
        {
            try
            {
                var (paymentIntent, message) = await this.paymentService.ProcessPaymentAsync(orderId);
                return Ok(new { message, PaymentIntentId = paymentIntent });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
