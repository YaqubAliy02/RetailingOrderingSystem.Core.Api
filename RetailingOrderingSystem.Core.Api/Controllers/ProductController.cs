using Application.UseCases.Products.Command;
using Application.UserCases.Products.Command;
using Application.UserCases.Products.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RetailingOrderingSystem.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public ProductController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddProductAsync([FromBody] CreateProductCommand command)
        {
            var result = await this.mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateProductByIdAsync([FromBody] UpdateProductCommand command)
        {
            return await this.mediator.Send(command);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteProductByIdAsync([FromQuery] DeleteProductCommand command)
        {
            return await this.mediator.Send(command);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            return await this.mediator.Send(new GetAllProductQuery());
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByIdAsync([FromQuery] GetProductByIdAsyncQuery getProductByIdAsyncQuery)
        {
            return await this.mediator.Send(getProductByIdAsyncQuery);
        }
    }
}
