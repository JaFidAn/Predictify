using BuildingBlocks.Exceptions.Handler;
using BuildingBlocks.Extensions;

namespace Prediction.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddCarterWithAssemblies(typeof(Program).Assembly);

            services.AddExceptionHandler<CustomExceptionHandler>();

            return services;
        }

        public static WebApplication UseApiServices(this WebApplication app)
        {
            app.MapCarter();

            app.UseExceptionHandler(options => { });

            return app;
        }
    }
}
