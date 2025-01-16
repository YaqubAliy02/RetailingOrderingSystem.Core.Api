using Application.Abstraction;
using Application.Repositories;
using Infrastracture.Services;
using Infrastructure.Data;
using Infrastructure.External.AWSS3;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            return services;
        }
    }
}
