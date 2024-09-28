using Microsoft.Extensions.Configuration;

namespace MoodCat.App.Common.BuildingBlocks.Extensions;

/// <summary>
/// Metody rozszerzające <see cref="IConfiguration"/>
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Zwraca connection string do bazy danych
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetDbConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("AppDbConnection")!;
    }

    /// <summary>
    /// Zwraca hasła przykładowych użytkowników
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static (string specialistPassword, string patientPassword) GetSeedUsersPasswords(
        this IConfiguration configuration)
    {
        var specialistPassword = configuration["Seed:SpecialistPassword"]!;
        var patientPassword = configuration["Seed:PatientPassword"]!;
        
        return (specialistPassword, patientPassword);
    }

    /// <summary>
    /// Zwraca kod API Key 
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetOpenAiApiKey(this IConfiguration configuration)
    {
        return configuration["OpenAi:ApiKey"]!;
    }
}