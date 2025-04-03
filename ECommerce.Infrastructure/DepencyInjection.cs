using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure
{
    public static class DepencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            SQLitePCL.Batteries.Init();

            services.AddDbContext<ECommerceDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
