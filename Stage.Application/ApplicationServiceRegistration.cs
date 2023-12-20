using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Stage.Application.Notifications;
using Stage.Application.Startup.Seeders;
using Stage.Application.UnitsOfWork;
using Stage.Domain.Notifications;
using Stage.Domain.UnitsOfWork;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Stage.Infrastructure.Persistence;

namespace Stage.Application
{
    public static partial class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //Classes
            services.AddTransient<IUnitOfWork, SqlUnitOfWork>();

            services.AddScoped<INotificationContext, NotificationContext>();

            //Services
            services.AddHostedService<InitialDataSeed>();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddFluentValidationAutoValidation((fv) =>
            {
                fv.DisableDataAnnotationsValidation = true;
            }).AddValidatorsFromAssemblyContaining<Assembly>(lifetime: ServiceLifetime.Transient);

            // Run Migrations
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<SqlContext>();
                try
                {
                    dbContext.Database.Migrate();
                    serviceProvider.MigrateDatabaseAsync<SqlContext>().Wait();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error applying migrations: {ex.Message}");
                }
            }

            return services;
        }

        public static async Task MigrateDatabaseAsync<T>(this IServiceProvider servicesProvider, int minutesTimeout = 5) where T : DbContext
        {
            using IServiceScope scope = servicesProvider.CreateScope();
            T requiredService = scope.ServiceProvider.GetRequiredService<T>();
            requiredService.Database.SetCommandTimeout((int)TimeSpan.FromMinutes(minutesTimeout).TotalMilliseconds);
            await requiredService.Database.MigrateAsync();
        }
    }
}
