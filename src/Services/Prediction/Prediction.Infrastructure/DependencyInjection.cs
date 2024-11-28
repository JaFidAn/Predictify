using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Prediction.Application.Data;
using Prediction.Application.Services;
using Prediction.Infrastructure.Data;
using Prediction.Infrastructure.Import;
using Prediction.Infrastructure.Interceptors;
using Prediction.Infrastructure.Services;

namespace Prediction.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            // Add services to the container
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, PublishDomainEventsInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            // Register the CountryApiService
            services.AddHttpClient<ICountryApiService, CountryApiService>(client =>
            {
                client.BaseAddress = new Uri("https://restcountries.com/v3.1/all");
                client.Timeout = TimeSpan.FromMinutes(2); // Increase timeout to 2 minutes
            });

            services.AddScoped<IFootballDataImporter, FootballDataImporter>();


            return services;
        }
    }
}
