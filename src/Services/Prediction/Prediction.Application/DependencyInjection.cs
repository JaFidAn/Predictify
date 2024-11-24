using BuildingBlocks.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Prediction.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });

            // Register all FluentValidation validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddExceptionHandler<CustomExceptionHandler>();

            return services;
        }
    }
}
