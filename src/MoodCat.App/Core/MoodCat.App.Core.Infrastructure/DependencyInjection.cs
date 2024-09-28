using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Domain.Users;
using MoodCat.App.Core.Infrastructure.Data;
using MoodCat.App.Core.Infrastructure.Data.Interceptors;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Infrastructure;

/// <summary>
/// Rozszerzenia IServiceCollection o dodawanie Layerów
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Dodaje warstwę infrastruktury do aplikacji razem z jej serwisami
    /// </summary>
    /// <param name="services">
    /// Serwisy aplikacji
    /// </param>
    /// <param name="configuration">
    /// Konfiguracja aplikacji          
    /// </param>
    /// <returns></returns>
    public static IServiceCollection AddInfrastructureLayerServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = 
            configuration.GetDbConnectionString();
        
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        // Konfiguracja kontekstu bazy danych SQL Server
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString);
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
        
        // Konfiguracja Microsoft Identity
        services.AddIdentityApiEndpoints<User>(opts =>
        {
            opts.User.RequireUniqueEmail = true;    
            opts.Password.RequiredLength = 6;
        }).AddEntityFrameworkStores<ApplicationDbContext>();
        
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>(); 

        return services;    
    }
}