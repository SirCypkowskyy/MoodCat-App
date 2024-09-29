using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Domain.Notes;
using MoodCat.App.Core.Domain.Notes.ValueObjects;
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

        var patientNotes1 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Dobry początek dnia"),
            NoteContent.Of(
                "Poranek był całkiem dobry. Czułem się wypoczęty po nocy, ale pod koniec dnia nastrój zaczął spadać."),
            6
        );
        patientNotes1.CreatedAt = DateTime.Now.AddDays(-7);
        patientNotes1.CreatedBy = "Seed";

        var patientNotes2 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Problemy z koncentracją"),
            NoteContent.Of(
                "Dziś ciężko mi było się skupić na pracy. Myśli błądziły gdzieś indziej, miałem wrażenie, że stoję w miejscu."),
            4
        );
        patientNotes2.CreatedAt = DateTime.Now.AddDays(-7);
        patientNotes2.CreatedBy = "Seed";

        var patientNotes3 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Spotkanie z przyjacielem"),
            NoteContent.Of(
                "Po południu spotkałem się z przyjacielem, co poprawiło mój nastrój. Śmiech i rozmowa dały mi dużo pozytywnej energii."),
            7
        );
        patientNotes3.CreatedAt = DateTime.Now.AddDays(-7);
        patientNotes3.CreatedBy = "Seed";

        var patientNotes4 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Walka z lękiem"),
            NoteContent.Of(
                "Wieczorem nasilił się mój lęk, miałem trudności z zasypianiem. Czułem się przytłoczony myślami o przyszłości."),
            3
        );
        patientNotes4.CreatedAt = DateTime.Now.AddDays(-7);
        patientNotes4.CreatedBy = "Seed";

// Dzień -6
        var patientNotes5 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Przeciętny dzień"),
            NoteContent.Of(
                "Dzień przebiegł dość przeciętnie. Nic specjalnego się nie wydarzyło, ale też nie było większych problemów."),
            5
        );
        patientNotes5.CreatedAt = DateTime.Now.AddDays(-6);
        patientNotes5.CreatedBy = "Seed";

        var patientNotes6 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Lekka apatia"),
            NoteContent.Of(
                "Czułem się apatyczny, brakowało mi motywacji, żeby cokolwiek zrobić. Próbowałem wyjść na spacer, ale to nie pomogło."),
            4
        );
        patientNotes6.CreatedAt = DateTime.Now.AddDays(-6);
        patientNotes6.CreatedBy = "Seed";

// Dzień -5
        var patientNotes7 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Niezłe samopoczucie"),
            NoteContent.Of("Dziś czułem się nieźle. Udało mi się załatwić kilka spraw, co poprawiło mój nastrój."),
            6
        );
        patientNotes7.CreatedAt = DateTime.Now.AddDays(-5);
        patientNotes7.CreatedBy = "Seed";

        var patientNotes8 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Nagły spadek nastroju"),
            NoteContent.Of("Popołudniu nagle poczułem się przytłoczony. Niewielkie rzeczy zaczęły mnie drażnić."),
            3
        );
        patientNotes8.CreatedAt = DateTime.Now.AddDays(-5);
        patientNotes8.CreatedBy = "Seed";

// Dzień -4
        var patientNotes9 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Trudny poranek"),
            NoteContent.Of("Rano miałem duże problemy z wstaniem z łóżka. Czułem się kompletnie bez sił."),
            2
        );
        patientNotes9.CreatedAt = DateTime.Now.AddDays(-4);
        patientNotes9.CreatedBy = "Seed";

        var patientNotes10 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Poprawa samopoczucia"),
            NoteContent.Of("Wieczorem, po rozmowie z terapeutą, poczułem lekką ulgę i nadzieję na poprawę."),
            5
        );
        patientNotes10.CreatedAt = DateTime.Now.AddDays(-4);
        patientNotes10.CreatedBy = "Seed";

// Dzień -3
        var patientNotes11 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Poczucie izolacji"),
            NoteContent.Of("Przez większość dnia czułem się bardzo samotny i odizolowany. Unikałem kontaktu z ludźmi."),
            3
        );
        patientNotes11.CreatedAt = DateTime.Now.AddDays(-3);
        patientNotes11.CreatedBy = "Seed";

        var patientNotes12 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Lepszy wieczór"),
            NoteContent.Of("Wieczorem obejrzałem film, który trochę mnie rozluźnił. Nie czułem już takiego napięcia."),
            4
        );
        patientNotes12.CreatedAt = DateTime.Now.AddDays(-3);
        patientNotes12.CreatedBy = "Seed";

// Dzień -2
        var patientNotes13 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Trudna rozmowa"),
            NoteContent.Of(
                "Dziś odbyłem trudną rozmowę z rodziną, co wywołało dużo stresu. Nie mogłem później się uspokoić."),
            2
        );
        patientNotes13.CreatedAt = DateTime.Now.AddDays(-2);
        patientNotes13.CreatedBy = "Seed";

        var patientNotes14 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Ćwiczenia na siłowni"),
            NoteContent.Of("Po rozmowie poszedłem na siłownię, co pomogło mi trochę uwolnić napięcie."),
            5
        );
        patientNotes14.CreatedAt = DateTime.Now.AddDays(-2);
        patientNotes14.CreatedBy = "Seed";

// Dzień -1
        var patientNotes15 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Spokojny dzień"),
            NoteContent.Of("Dzień przebiegł spokojnie. Dużo czasu spędziłem na czytaniu książek i relaksie."),
            6
        );
        patientNotes15.CreatedAt = DateTime.Now.AddDays(-1);
        patientNotes15.CreatedBy = "Seed";

        var patientNotes16 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Rozmowa z terapeutą"),
            NoteContent.Of("Po sesji terapeutycznej czułem się nieco lżej, choć wciąż mam sporo wątpliwości."),
            5
        );
        patientNotes16.CreatedAt = DateTime.Now.AddDays(-1);
        patientNotes16.CreatedBy = "Seed";
        
        var patientNotes17 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Zmęczenie od rana"),
            NoteContent.Of("Od samego rana czułem się zmęczony, jakby cały dzień miał być z góry stracony."),
            3
        );
        patientNotes17.CreatedAt = DateTime.Now.AddDays(-8);
        patientNotes17.CreatedBy = "Seed";

        var patientNotes18 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Wizyta u lekarza"),
            NoteContent.Of(
                "Wizyta u lekarza okazała się bardziej stresująca niż się spodziewałem. Czułem, że nie mam pełnej kontroli nad swoim stanem."),
            4
        );
        patientNotes18.CreatedAt = DateTime.Now.AddDays(-8);
        patientNotes18.CreatedBy = "Seed";

// Dzień -9
        var patientNotes19 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Trudności z porankiem"),
            NoteContent.Of("Znowu miałem problemy ze wstawaniem z łóżka. Czułem, że jestem przytłoczony już od rana."),
            2
        );
        patientNotes19.CreatedAt = DateTime.Now.AddDays(-9);
        patientNotes19.CreatedBy = "Seed";

        var patientNotes20 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Rozmowa telefoniczna z mamą"),
            NoteContent.Of(
                "Rozmowa z mamą poprawiła mi trochę nastrój. Czułem się bardziej zrozumiany, ale wciąż ciężko było mi wyjść z domu."),
            5
        );
        patientNotes20.CreatedAt = DateTime.Now.AddDays(-9);
        patientNotes20.CreatedBy = "Seed";

// Dzień -10
        var patientNotes21 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Deszczowy dzień, niskie samopoczucie"),
            NoteContent.Of("Cały dzień padało, co odbiło się na moim samopoczuciu. Spędziłem większość dnia w łóżku."),
            3
        );
        patientNotes21.CreatedAt = DateTime.Now.AddDays(-10);
        patientNotes21.CreatedBy = "Seed";

        var patientNotes22 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Spacer w deszczu"),
            NoteContent.Of(
                "Wyszedłem na krótki spacer mimo deszczu, co pomogło mi na chwilę odetchnąć, ale nastrój nie poprawił się znacząco."),
            4
        );
        patientNotes22.CreatedAt = DateTime.Now.AddDays(-10);
        patientNotes22.CreatedBy = "Seed";

// Dzień -11
        var patientNotes23 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Niezdecydowanie i frustracja"),
            NoteContent.Of(
                "Miałem wrażenie, że nie mogę się na nic zdecydować. Każda decyzja, nawet błaha, wywoływała frustrację."),
            3
        );
        patientNotes23.CreatedAt = DateTime.Now.AddDays(-11);
        patientNotes23.CreatedBy = "Seed";

        var patientNotes24 = NoteEntity.Create(patient.Id,
            NoteTitle.Of("Chwila relaksu"),
            NoteContent.Of("Pod wieczór wziąłem gorącą kąpiel, co na chwilę pomogło mi zrelaksować się."),
            5
        );
        patientNotes24.CreatedAt = DateTime.Now.AddDays(-11);
        patientNotes24.CreatedBy = "Seed";

        // Dodanie wszystkich notatek do kontekstu
        await context.Notes.AddRangeAsync(
            patientNotes1, patientNotes2, patientNotes3, patientNotes4,
            patientNotes5, patientNotes6, patientNotes7, patientNotes8,
            patientNotes9, patientNotes10, patientNotes11, patientNotes12,
            patientNotes13, patientNotes14, patientNotes15, patientNotes16,
            patientNotes17, patientNotes18, patientNotes19, patientNotes20,
            patientNotes21, patientNotes22, patientNotes23, patientNotes24
        );

        await context.SaveChangesAsync();


        await context.SaveChangesAsync();


        await context.SaveChangesAsync();

        await transaction.CommitAsync();
    }
}