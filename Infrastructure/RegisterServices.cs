﻿using System.Text;
using Application.Abstraction;
using Application.Repositories;
using Application.Repository;
using Infrastracture.Repositories;
using Infrastructure.Data;
using Infrastructure.External.AWSS3;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure
{
    public static class RegisterServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<IRetailingOrderingSystemDbContext, RetailingOrderingSystemDbContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductThumbnailRepository, ProductThumbnailRepository>();
            services.AddScoped<IAWSStorage, AWSStorage>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.SaveToken = true;
                   options.TokenValidationParameters = new()
                   {
                       ValidateIssuer = true,
                       ValidateAudience = true,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       ValidAudience = configuration["JWT:AudienceKey"],
                       ValidIssuer = configuration["JWT:IssuerKey"],
                       IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                       ClockSkew = TimeSpan.Zero
                   };
                   options.Events = new JwtBearerEvents
                   {
                       OnAuthenticationFailed = (context) =>
                       {
                           if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                           {
                               context.Response.Headers["IsTokenExpired"] = "true";
                           }
                           return Task.CompletedTask;
                       }
                   };
               });
            return services;
        }
    }
}
