using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MoodCat.App.Common.BuildingBlocks.Exceptions.Handler;
using MoodCat.App.Common.BuildingBlocks.Extensions;

namespace MoodCat.App.Core.WebAPI;

public static class DependencyInjection
{
    /// <summary>
    /// Dodaje warstwę api i jej serwisy
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddApiLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetDbConnectionString());
        return services;
    }

    /// <summary>
    /// Używa serwisów z warstwy API
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseApiLayerServices(this WebApplication app)
    {
        app.MapCarter();

        app.UseExceptionHandler(opts => { });

        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}