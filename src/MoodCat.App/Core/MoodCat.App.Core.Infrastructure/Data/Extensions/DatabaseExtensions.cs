using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Domain.Users;

namespace MoodCat.App.Core.Infrastructure.Data.Extensions;

/// <summary>
/// Metody rozszerające bazę danych
/// </summary>
public static class DatabaseExtensions
{
    /// <summary>
    /// Inicjalizuje bazę danych
    /// </summary>
    /// <param name="app"></param>
    /// <param name="configuration"></param>
    public static async Task InitializeDatabaseAsync(this WebApplication app, IConfiguration configuration)
    {
        await using var scope = app.Services.CreateAsyncScope();

        await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        
        logger.LogInformation("Initializing database ...");
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migrated");
        await SeedDataAsync(context, configuration, logger);
    }

    /// <summary>
    /// Seeduje bazę danych
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    private static async Task SeedDataAsync(ApplicationDbContext context, IConfiguration configuration, ILogger logger)
    {
        logger.LogInformation("Seeding data...");

        if (await context.Users.AnyAsync())
        {
            logger.LogWarning("User already exists, skipping seeding");
            return;
        }

        await using var transaction = await context.Database.BeginTransactionAsync();

        var roles = new IdentityRole[]
        {
            new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new()
            {
                Name = "Moderator",
                NormalizedName = "MODERATOR"
            },
            new()
            {
                Name = "Specialist",
                NormalizedName = "SPECIALIST"
            },
            new()
            {
                Name = "User",
                NormalizedName = "USER"
            }
        };

        logger.LogInformation("Adding roles: {roles}", roles.Select(r => r.Name));

        await context.Roles.AddRangeAsync(roles);

        var hasher = new PasswordHasher<User>();

        var passwords = configuration.GetSeedUsersPasswords();

        var specialist = new User()
        {
            UserName = "specialist",
            NormalizedUserName = "SPECIALIST",
            Email = "specialist@mood.cat",
            NormalizedEmail = "SPECIALIST@MOOD.CAT",
            PasswordHash = hasher.HashPassword(null, passwords.specialistPassword),
            EmailConfirmed = true,
            LockoutEnabled = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var patient = new User()
        {
            UserName = "patient",
            NormalizedUserName = "PATIENT",
            Email = "patient@mood.cat",
            NormalizedEmail = "PATIENT@MOOD.CAT",
            PasswordHash = hasher.HashPassword(null, passwords.patientPassword),
            EmailConfirmed = true,
            LockoutEnabled = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString()
        };
        
        logger.LogInformation("Adding specialists: {specialists}", specialist.UserName);
        
        await context.Users.AddAsync(patient);
        await context.Users.AddAsync(specialist);

        await context.SaveChangesAsync();

        await context.UserRoles.AddRangeAsync(new[]
        {
            new IdentityUserRole<string>()
            {                       
                RoleId = roles[2].Id,
                UserId = specialist.Id
            },
            new IdentityUserRole<string>()
            {
                RoleId = roles[3].Id,
                UserId = patient.Id
            }
        });
        
        await context.SaveChangesAsync();
        
        patient.AssignedSpecialistId = specialist.Id;
        
        await context.SaveChangesAsync();
        
        await transaction.CommitAsync();
    }
}