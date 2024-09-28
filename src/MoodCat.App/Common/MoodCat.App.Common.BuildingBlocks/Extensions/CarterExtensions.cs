using System.Reflection;
using Carter;
using Microsoft.Extensions.DependencyInjection;

namespace MoodCat.App.Common.BuildingBlocks.Extensions;

/// <summary>
/// Klasa zawierająca metody rozszerzające dla Cartera
/// </summary>
public static class CarterExtensions
{
    /// <summary>
    /// Metoda dodająca Cartera z modułami z podanych assembly
    /// </summary>
    /// <param name="services">
    /// Kolekcja serwisów, do której dodawany jest Carter
    /// </param>
    /// <param name="assemblies">
    /// Lista assembly, z których mają być pobrane moduły
    /// </param>
    /// <returns></returns>
    public static IServiceCollection AddCarterWithAssemblies
        (this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddCarter(configurator: config =>
        {
            foreach (var assembly in assemblies)
            {
                var modules = assembly.GetTypes()
                    .Where(t => t.IsAssignableTo(typeof(ICarterModule))).ToArray();

                config.WithModules(modules);
            }
        });
        return services;
    }
}