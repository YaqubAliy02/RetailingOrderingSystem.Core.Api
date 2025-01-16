using Application.Abstraction;
using Application.Models;
using Application.Repositories;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using MediatR;

namespace Application.UserCases.Products.Command
{
    public class CreateProductCommand : IRequest<ResponseCore<CreateProductCommandHandlerResult>>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ResponseCore<CreateProductCommandHandlerResult>>
    {
        private readonly IMapper mapper;
        private readonly IRetailingOrderingSystemDbContext context;
        private readonly IProductRepository productRepository;
        private IValidator<Product> validator;

        public CreateProductCommandHandler(IMapper mapper,
            IRetailingOrderingSystemDbContext context,
            IProductRepository productRepository,
            IValidator<Product> validator)
        {
            this.mapper = mapper;
            this.context = context;
            this.productRepository = productRepository;
            this.validator = validator;
        }

        public async Task<ResponseCore<CreateProductCommandHandlerResult>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var result = new ResponseCore<CreateProductCommandHandlerResult>();

            Product product = this.mapper.Map<Product>(request);
            var validationResult = this.validator.Validate(product);

            if (!validationResult.IsValid)
            {
                result.ErrorMessage = validationResult.Errors.ToArray();
                result.StatusCode = 400;

                return result;
            }

            if (product is null)
            {
                result.ErrorMessage = new string[] { "Product is not found" };
                result.StatusCode = 404;
                return result;
            }

            product = await this.productRepository.AddAsync(product);

            result.Result = this.mapper.Map<CreateProductCommandHandlerResult>(product);
            result.StatusCode = 200;

            return result;
        }
    }

    public class CreateProductCommandHandlerResult
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }

    }
}
