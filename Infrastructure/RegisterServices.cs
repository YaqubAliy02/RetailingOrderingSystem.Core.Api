using Application.Abstraction;
using Application.Repositories;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
