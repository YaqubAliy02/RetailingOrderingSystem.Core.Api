using Domain.Models;
using FluentValidation;

public class UserValidation : AbstractValidator<User>
{
    public UserValidation()
    {
        RuleFor(user => user.FirstName).NotEmpty().WithMessage("UserName is required!");
        RuleFor(user => user.LastName).NotEmpty().WithMessage("LastName is required!");
        RuleFor(user => user.Role).NotEmpty().WithMessage($"{nameof(User.Role)}");
        RuleFor(user => user.Email).NotEmpty().EmailAddress().WithMessage("Please provide a valid email address in the format: 'example@example.com'. ");
    }
}