using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UserCases.ProductThumbnails.Command
{
    public class DeleteProductThumbnailCommand: IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class DeleteProductThumbnailCommandHandler : IRequestHandler<DeleteProductThumbnailCommand, IActionResult>
    {
        private readonly IProductThumbnailRepository productThumbnailRepository;

        public DeleteProductThumbnailCommandHandler(IProductThumbnailRepository productThumbnailRepository)
        {
            this.productThumbnailRepository = productThumbnailRepository;
        }

        public async Task<IActionResult> Handle(DeleteProductThumbnailCommand request, CancellationToken cancellationToken)
        {
             bool result = await this.productThumbnailRepository.DeleteAsync(request.Id);
            if(!result)
                return new NotFoundObjectResult("Delete operation is failed");

            return new OkObjectResult("Product Thumbnail deleted successfully");
        }
    }
}
