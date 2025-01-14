using System.Threading.Tasks;
using Application.UserCases.Products.Command;
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
    }
}
