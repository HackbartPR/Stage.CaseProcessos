using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stage.Domain.Config;

namespace Stage.Infrastructure
{
    public static class InsfrastructureServiceRegistration
    {
        public static IServiceCollection RegisterDatabase<T>(this IServiceCollection services) where T : DbContext
        {
            return services.AddDbContext<T>(options =>
            {
                options.UseSqlServer(Settings.Database.ConnectionString);
                options.UseLazyLoadingProxies();
            });
        }
    }
}
