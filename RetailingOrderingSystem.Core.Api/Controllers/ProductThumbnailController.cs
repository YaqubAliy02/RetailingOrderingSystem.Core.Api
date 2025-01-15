using Application.UserCases.ProductThumbnails.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RetailingOrderingSystem.Core.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductThumbnailController : ApiControllerBase
    {
        private readonly IMediator mediator;

        public ProductThumbnailController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UploadProductThumbnail([FromQuery] CreateProductThumbnailCommand command)
        {
            return await this.mediator.Send(command);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteProductThumbnail([FromQuery] DeleteProductThumbnailCommand command)
        {
            return await this.mediator.Send(command);
        }
    }
}
