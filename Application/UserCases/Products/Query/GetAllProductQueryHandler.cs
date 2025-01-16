using Application.DTOs.Products;
using Application.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UserCases.Products.Query
{
    public class GetAllProductQuery : IRequest<IActionResult> { }
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IActionResult>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        public GetAllProductQueryHandler(IProductRepository productRepository,
            IMapper mapper)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var products = await productRepository.GetAllAsync(x => true);

            if (products is null)
                return new NotFoundObjectResult("No products found");

            var productList = await products.ToListAsync();

            var productDto = this.mapper.Map<List<GetProductDto>>(productList);

            return new OkObjectResult(productDto);
        }
    }
}

