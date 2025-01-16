﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Abstraction;
using Application.DTOs.Users;
using Application.Extensions;
using Application.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Accounts.Command
{
    public class LoginUserCommand : IRequest<IActionResult>
    {
        [EmailAddress]
        public string Email { get; set; }
        [PasswordPropertyText]
        public string Password { get; set; }
    }
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IActionResult>
    {
        private readonly IUserRepository userRepository;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        public LoginUserCommandHandler(
            IUserRepository userRepository,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await this.userRepository
              .GetAllAsync(x => x.Password == request.Password.GetHash()
                 && x.Email == request.Email).Result.FirstOrDefaultAsync();

            var userGetDto = this.mapper.Map<UserGetDto>(user);

            if (user is not null)
            {
                UserRegisterDto userRegisterDto = new UserRegisterDto()
                {
                    User = userGetDto,
                    UserToken = await this.tokenService.CreateTokenAsync(user),
                };
                return new OkObjectResult(userRegisterDto);
            }
            return new BadRequestObjectResult("Email or Password is incorrect :( ");
        }
    }
}
