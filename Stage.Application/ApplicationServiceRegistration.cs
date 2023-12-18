using Microsoft.Extensions.DependencyInjection;
using Stage.Application.Notifications;
using Stage.Application.Startup.Seeders;
using Stage.Application.UnitsOfWork;
using Stage.Domain.Notifications;
using Stage.Domain.UnitsOfWork;
using System.Reflection;

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

            return services;
        }
    }
}
