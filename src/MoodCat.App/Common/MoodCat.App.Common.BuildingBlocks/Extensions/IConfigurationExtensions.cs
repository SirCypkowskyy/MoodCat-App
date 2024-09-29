using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
        try
        {
            return configuration["ConnectionStrings:AppDbConnection"] 
                   ?? throw new NullReferenceException("Nie znaleziono ConnectionString w appsettingsach!");
        }
        catch (Exception e)
        {
            return Environment.GetEnvironmentVariable("ConnectionStrings_AppDbConnection")!;
        }
    }

    /// <summary>
    /// Zwraca hasła przykładowych użytkowników
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static (string specialistPassword, string patientPassword) GetSeedUsersPasswords(
        this IConfiguration configuration)
    {
        try
        {

            var specialistPassword = configuration["Seed:SpecialistPassword"]!;
            var patientPassword = configuration["Seed:PatientPassword"]!;

            return (specialistPassword, patientPassword);
        }
        catch (Exception e)
        {
            var specialistPassword = Environment.GetEnvironmentVariable("Seed_SpecialistPassword")!;
            var patientPassword = Environment.GetEnvironmentVariable("Seed_PatientPassword")!;
            return (specialistPassword, patientPassword);
        }
    }

    /// <summary>
    /// Zwraca kod API Key 
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetOpenAiApiKey(this IConfiguration configuration)
    {
        try
        {
            return configuration["OpenAi:ApiKey"]!;
        }
        catch (Exception e)
        {
            return Environment.GetEnvironmentVariable("OpenAi_ApiKey")!;
        }
    }

    /// <summary>
    /// Zwraca model GPT, używany przez aplikację
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static string GetOpenAiGptModel(this IConfiguration configuration)
    {
        try
        {       
            return configuration["OpenAi:GptModel"]!;
        }
        catch (Exception e)
        {
            return Environment.GetEnvironmentVariable("OpenAi_GptModel")!;
        }
    }
}