using Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UserCases.Products.Query
{
    public class GetAllProductQuery : IRequest<IActionResult> { }
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IActionResult>
    {
        private readonly IProductRepository productRepository;

        public GetAllProductQueryHandler(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var products = await productRepository.GetAllAsync(x => true);

            if(products is null)
                return new NotFoundObjectResult("No products found");

            return new OkObjectResult(products);
        }
    }
}
