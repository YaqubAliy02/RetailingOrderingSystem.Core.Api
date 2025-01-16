using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Abstraction;
using Application.DTOs.Users;
using Application.Repositories;
using AutoMapper;
using Domain.Enums;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.Users.Command
{
    public class RegisterUserCommand : IRequest<IActionResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
        public Role Role { get; set; }

    }
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IActionResult>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly IValidator<User> validator;
        private readonly ITokenService tokenService;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IMapper mapper,
            IValidator<User> validator,
            ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.validator = validator;
            this.tokenService = tokenService;
        }

        public async Task<IActionResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var users = await this.userRepository.GetAllAsync(x => true);

            if (users.Any(u => u.Email == request.Email))
                return new BadRequestObjectResult("Email already exists");

            var user = mapper.Map<User>(request);

            var validationResult = validator.Validate(user);
            if (!validationResult.IsValid)
                return new BadRequestObjectResult(validationResult.Errors);

            user.Role = request.Role;

            user = await this.userRepository.AddAsync(user);

            var userGetDto = mapper.Map<UserGetDto>(user);

            if (user != null)
            {
                var userRegisterDto = new UserRegisterDto
                {
                    User = userGetDto,
                    UserToken = await tokenService.CreateTokenAsync(user)
                };

                return new OkObjectResult(userRegisterDto);
            }

            return new BadRequestObjectResult("User is not valid!");
        }

    }

}
