using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UserCases.Products.Command
{
    public class DeleteProductCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, IActionResult>
    {
        private readonly IProductRepository productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            bool isDeleted = await productRepository.DeleteAsync(request.Id);
            if (isDeleted)
                return new OkObjectResult("Product deleted successfully");

            return new NotFoundObjectResult("Product not found");
        }
    }
}
