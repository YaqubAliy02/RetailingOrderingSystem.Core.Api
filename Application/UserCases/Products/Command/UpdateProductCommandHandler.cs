using Application.DTOs.Products;
using Application.Repositories;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Products.Command
{
    public class UpdateProductCommand : IRequest<IActionResult>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
        public Guid[] ThumbnailsId { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, IActionResult>
    {
        private readonly IMapper mapper;
        private readonly IProductRepository productRepository;
        private readonly IValidator<Product> validator;

        public UpdateProductCommandHandler(
            IMapper mapper,
            IProductRepository productRepository,
            IValidator<Product> validator
            )
        {
            this.mapper = mapper;
            this.productRepository = productRepository;
            this.validator = validator;
        }

        public async Task<IActionResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await productRepository.GetByIdAsync(request.Id);
            if (existingProduct is null)
                return new NotFoundObjectResult("Product not found");


            mapper.Map(request, existingProduct);

            var validationResult = validator.Validate(existingProduct);

            if (!validationResult.IsValid)
                return new BadRequestObjectResult(validationResult);

            var updatedProduct = await productRepository.UpdateAsync(existingProduct);
            var productDto = mapper.Map<UpdateProductDTO>(updatedProduct);    
            return new OkObjectResult(updatedProduct);
        }
    }
}

