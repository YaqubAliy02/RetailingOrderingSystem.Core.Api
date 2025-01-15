using Application.DTOs.Products;
using Application.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UserCases.Products.Query
{
    public class GetProductByIdAsyncQuery : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
    }
    public class GetProductByIdAsyncQueryHandler : IRequestHandler<GetProductByIdAsyncQuery, IActionResult>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;

        public GetProductByIdAsyncQueryHandler(IProductRepository productRepository,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetProductByIdAsyncQuery request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByIdAsync(request.Id);

            if (product is null)
                return new NotFoundObjectResult("Product not found");

            var productDto = this.mapper.Map<GetProductDto>(product);

            return new OkObjectResult(productDto);
        }
    }
}

