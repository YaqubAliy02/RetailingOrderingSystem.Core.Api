﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using System.Data;
using Application.Mappings;
namespace Application
{
    public static class RegisterServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}