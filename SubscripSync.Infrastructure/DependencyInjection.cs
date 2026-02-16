using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SubscripSync.Domain.Interfaces;
using SubscripSync.Infrastructure.Data;
using SubscripSync.Infrastructure.Repositories;

namespace SubscripSync.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<SubscripSyncDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(SubscripSyncDbContext).Assembly.FullName)));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddHostedService<Services.SubscriptionRenewalWorker>();

            return services;
        }
    }
}
