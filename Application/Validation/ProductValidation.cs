using Domain.Models;
using FluentValidation;

namespace Application.Validation
{
    public class ProductValidation : AbstractValidator<Product>
    {
        public ProductValidation()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(p => p.Price).GreaterThan(0);
            RuleFor(p => p.Stock).GreaterThan(0);
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("Category is required");
        }
    }
}
