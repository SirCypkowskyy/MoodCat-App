using System.Text.Json;
using System.Text.RegularExpressions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Exceptions;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Application.DTOs.DaySummaries;
using MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;
using MoodCat.App.Core.Application.Interfaces;
using MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;
using MoodCat.App.Core.Domain.DaySummaries;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.DaySummaries.Commands.GenerateSummarizeWeek;

/// <summary>
/// Handler komendy od generowania podsumowania tygodnia
/// </summary>
public partial class GenerateSummarizeWeekHandler(
    IApplicationDbContext dbContext,
    IChatGptService chatGptService,
    IConfiguration config,
    ILogger<GenerateSummarizeWeekHandler> logger)
    : IRequestHandler<GenerateSummarizeWeekCommand, GenerateSummarizeWeekResult>
{
    /// <inheritdoc />
    public async Task<GenerateSummarizeWeekResult> Handle(GenerateSummarizeWeekCommand command,
        CancellationToken cancellationToken)
    {
        // Znajdujemy ostatnią niedzielę
        var today = DateTime.Now;
        
        var last7Days = today.AddDays(-7);
        // var lastSunday = today.AddDays(-(int)today.DayOfWeek); // ostatnia niedziela (włącznie z dzisiaj, jeśli to niedziela)
        // var lastMonday = lastSunday.AddDays(-6); // ostatni poniedziałek

        // Sprawdzamy, czy istnieje już podsumowanie dla tego tygodnia
        var existingSummaryForTheWeek = await dbContext.DaysSummaries
            .FirstOrDefaultAsync(x =>
                    x.SummaryDate >= DateOnly.FromDateTime(last7Days) && 
                    // x.SummaryDate <= DateOnly.FromDateTime(lastSunday) && 
                    x.UserId == command.UserId,
                cancellationToken: cancellationToken);

        var summaryForTheWeekExists = existingSummaryForTheWeek != null;

        if (summaryForTheWeekExists && command.ForceRefresh != true)
            return new GenerateSummarizeWeekResult(
                new SummarizeResultDTO(
                    command.UserId,
                    existingSummaryForTheWeek.Content,
                    existingSummaryForTheWeek.OriginalContent,
                    (decimal)existingSummaryForTheWeek.HappinessLevel,
                    existingSummaryForTheWeek.PatientGeneralFunctioning,
                    existingSummaryForTheWeek.OriginalPatientGeneralFunctioning,
                    existingSummaryForTheWeek.Interests,
                    existingSummaryForTheWeek.OriginalInterests,
                    existingSummaryForTheWeek.SocialRelationships,
                    existingSummaryForTheWeek.OriginalSocialRelationships,
                    existingSummaryForTheWeek.Work,
                    existingSummaryForTheWeek.OriginalWork,
                    existingSummaryForTheWeek.Family,
                    existingSummaryForTheWeek.OriginalFamily,
                    existingSummaryForTheWeek.PhysicalHealth,
                    existingSummaryForTheWeek.OriginalPhysicalHealth,
                    existingSummaryForTheWeek.Memories,
                    existingSummaryForTheWeek.OriginalMemories,
                    existingSummaryForTheWeek.ReportedProblems,
                    existingSummaryForTheWeek.OriginalReportedProblems,
                    existingSummaryForTheWeek.Other,
                    existingSummaryForTheWeek.OriginalOther
                )
            );

        // Pobieramy notatki z ostatniego tygodnia
        var properNotes = await dbContext.Notes
            .Where(x => x.UserId == command.UserId && 
                        x.CreatedAt.HasValue && 
                        x.CreatedAt.Value.Date >= last7Days
                        //x.CreatedAt.Value.Date >= lastMonday.Date && 
                       // x.CreatedAt.Value.Date <= lastSunday.Date
                       )
            .ToListAsync(cancellationToken: cancellationToken);

        // Jeśli nie ma żadnych notatek, wyrzuć wyjątek
        if (properNotes is null || properNotes.Count == 0)
            throw new NotFoundException("Week summary couldn't be created, because user didn't create any notes in the last week");

        // Łączenie zawartości notatek
        var contentMerged = properNotes
            .Select(x => x.Content.Value)
            .Aggregate((x, y) => $"{x}\n{y}");

        // Generowanie odpowiedzi z ChatGPT
        var completedMessage = await chatGptService.GenerateResponse(
            new SendGptPromptCommand(
                new ChatGptRequestDTO(config.GetOpenAiGptModel(), contentMerged)
            ),
            cancellationToken
        );

        DaySummaryEntity? deserializedWeekSummary = null;
        try
        {
            var arrayOfStrings = completedMessage.Content.Select(x => x.Text).ToArray();

            var joined = string.Join("", arrayOfStrings);
            
            // Walidacja, że stworzony string to json
            var isProperJson = JsonRegex().IsMatch(joined);

            if (!isProperJson)
                throw new InternalServerException(
                    "Wygenerowane podsumowanie tygodnia nie jest zapisane w odpowiednim formacie JSON!");
            
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(joined)!;

            if (summaryForTheWeekExists)
            {
                existingSummaryForTheWeek.TryToUpdateWithDict(dict);
                existingSummaryForTheWeek.UpdateUserId(command.UserId);
            }
            else
            {
                deserializedWeekSummary = new DaySummaryEntity();

                deserializedWeekSummary.TryToUpdateWithDict(dict);

                deserializedWeekSummary?.UpdateUserId(command.UserId);
            }
        }
        catch (Exception e)
        {
            logger.LogError("Podczas próby deserializacji WeekSummary doszło do błędu.");
            throw;
        }

        // Zapisanie podsumowania tygodnia do bazy danych
        if (deserializedWeekSummary is not null)
            await dbContext.DaysSummaries.AddAsync(deserializedWeekSummary, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        // Aktualizacja notatek
        if (deserializedWeekSummary is not null)
        {
            foreach (var properNote in properNotes)
                properNote.UpdateDaySummaryId(deserializedWeekSummary.Id);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        // Zwracanie wyniku
        if (deserializedWeekSummary is not null)
        {
            return new GenerateSummarizeWeekResult(
                new SummarizeResultDTO(
                    command.UserId,
                    deserializedWeekSummary.Content,
                    deserializedWeekSummary.OriginalContent,
                    (decimal)deserializedWeekSummary.HappinessLevel,
                    deserializedWeekSummary.PatientGeneralFunctioning,
                    deserializedWeekSummary.OriginalPatientGeneralFunctioning,
                    deserializedWeekSummary.Interests,
                    deserializedWeekSummary.OriginalInterests,
                    deserializedWeekSummary.SocialRelationships,
                    deserializedWeekSummary.OriginalSocialRelationships,
                    deserializedWeekSummary.Work,
                    deserializedWeekSummary.OriginalWork,
                    deserializedWeekSummary.Family,
                    deserializedWeekSummary.OriginalFamily,
                    deserializedWeekSummary.PhysicalHealth,
                    deserializedWeekSummary.OriginalPhysicalHealth,
                    deserializedWeekSummary.Memories,
                    deserializedWeekSummary.OriginalMemories,
                    deserializedWeekSummary.ReportedProblems,
                    deserializedWeekSummary.OriginalReportedProblems,
                    deserializedWeekSummary.Other,
                    deserializedWeekSummary.OriginalOther
                )
            );
        }

        return new GenerateSummarizeWeekResult(
            new SummarizeResultDTO(
                command.UserId,
                existingSummaryForTheWeek.Content,
                existingSummaryForTheWeek.OriginalContent,
                (decimal)existingSummaryForTheWeek.HappinessLevel,
                existingSummaryForTheWeek.PatientGeneralFunctioning,
                existingSummaryForTheWeek.OriginalPatientGeneralFunctioning,
                existingSummaryForTheWeek.Interests,
                existingSummaryForTheWeek.OriginalInterests,
                existingSummaryForTheWeek.SocialRelationships,
                existingSummaryForTheWeek.OriginalSocialRelationships,
                existingSummaryForTheWeek.Work,
                existingSummaryForTheWeek.OriginalWork,
                existingSummaryForTheWeek.Family,
                existingSummaryForTheWeek.OriginalFamily,
                existingSummaryForTheWeek.PhysicalHealth,
                existingSummaryForTheWeek.OriginalPhysicalHealth,
                existingSummaryForTheWeek.Memories,
                existingSummaryForTheWeek.OriginalMemories,
                existingSummaryForTheWeek.ReportedProblems,
                existingSummaryForTheWeek.OriginalReportedProblems,
                existingSummaryForTheWeek.Other,
                existingSummaryForTheWeek.OriginalOther
            )
        );
    }
    
    private const string JsonPattern = "(?<json>{(?:[^{}]|(?<Nested>{)|(?<-Nested>}))*(?(Nested)(?!))})";

    [GeneratedRegex(JsonPattern)]
    private static partial Regex JsonRegex();
}
