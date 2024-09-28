using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MoodCat.App.Common.BuildingBlocks.Behaviors;

namespace MoodCat.App.Core.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Dodaje layer aplikacji z jej serwisami
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddApplicationLayerServices(this IServiceCollection services)
    {                   
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        

        services.AddHttpClient();
        
        return services;
    }
}