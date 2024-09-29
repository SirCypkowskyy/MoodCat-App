using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Application.DTOs.DaySummaries;
using MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;
using MoodCat.App.Core.Application.Interfaces;
using MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;
using MoodCat.App.Core.Domain.DaySummaries;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.DaySummaries.Commands.GenerateSummarizeDay;

/// <summary>
/// Handler komendy od generowania podsumowania dnia
/// </summary>
public class GenerateSummarizeDayHandler(
    IApplicationDbContext dbContext,
    IChatGptService chatGptService,
    IConfiguration config,
    ILogger<GenerateSummarizeDayHandler> logger)
    : IRequestHandler<GenerateSummarizeDayCommand, GenerateSummarizeDayResult>
{
    /// <inheritdoc />
    public async Task<GenerateSummarizeDayResult> Handle(GenerateSummarizeDayCommand command,
        CancellationToken cancellationToken)
    {
        var dateOnlyToday = DateOnly.FromDateTime(DateTime.Now);
        var existingSummaryForTheDay = await dbContext.DaysSummaries
            .FirstOrDefaultAsync(x => x.SummaryDate == dateOnlyToday, cancellationToken: cancellationToken);

        var summaryForTheDayExists = existingSummaryForTheDay != null;

        if (summaryForTheDayExists && command.ForceRefresh != true)
            return new GenerateSummarizeDayResult(
                new DaySummarizeResultDTO(
                    command.UserId,
                    existingSummaryForTheDay.Content,
                    existingSummaryForTheDay.OriginalContent,
                    (decimal)existingSummaryForTheDay.HappinessLevel,
                    existingSummaryForTheDay.PatientGeneralFunctioning,
                    existingSummaryForTheDay.OriginalPatientGeneralFunctioning,
                    existingSummaryForTheDay.Interests,
                    existingSummaryForTheDay.OriginalInterests,
                    existingSummaryForTheDay.SocialRelationships,
                    existingSummaryForTheDay.OriginalSocialRelationships,
                    existingSummaryForTheDay.Work,
                    existingSummaryForTheDay.OriginalWork,
                    existingSummaryForTheDay.Family,
                    existingSummaryForTheDay.OriginalFamily,
                    existingSummaryForTheDay.PhysicalHealth,
                    existingSummaryForTheDay.OriginalPhysicalHealth,
                    existingSummaryForTheDay.Memories,
                    existingSummaryForTheDay.OriginalMemories,
                    existingSummaryForTheDay.ReportedProblems,
                    existingSummaryForTheDay.OriginalReportedProblems,
                    existingSummaryForTheDay.Other,
                    existingSummaryForTheDay.OriginalOther
                )
            );

        var properNotes = await dbContext.Notes
            .Where(x => x.UserId == command.UserId && x.CreatedAt!.Value.Date == DateTime.Now.Date)
            .ToListAsync(cancellationToken: cancellationToken);

        var contentMerged = properNotes
            .Select(x => x.Content.Value)
            .Aggregate((x, y) => $"{x}\n{y}");

        var completedMessage = await chatGptService.GenerateResponse(
            new SendGptPromptCommand(
                new ChatGptRequestDTO(config.GetOpenAiGptModel(), contentMerged)
            ),
            cancellationToken
        );

        DaySummaryEntity? deserializedDaySummary = null;
        try
        {
            var arrayOfStrings = completedMessage.Content.Select(x => x.Text).ToArray();

            var joined = string.Join("", arrayOfStrings);
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(joined)!;

            if (summaryForTheDayExists)
            {
                existingSummaryForTheDay.TryToUpdateWithDict(dict);
                existingSummaryForTheDay.UpdateUserId(command.UserId);
            }
            else
            {
                deserializedDaySummary = new DaySummaryEntity();

                deserializedDaySummary.TryToUpdateWithDict(dict);

                deserializedDaySummary?.UpdateUserId(command.UserId);
            }
        }
        catch (Exception e)
        {
            logger.LogError("Podczas próby deserializacji DaySummary doszło do błędu.");
            throw;
        }

        if (deserializedDaySummary is not null)
            await dbContext.DaysSummaries.AddAsync(deserializedDaySummary, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        if (deserializedDaySummary is not null)
        {
            foreach (var properNote in properNotes)
                properNote.UpdateDaySummaryId(deserializedDaySummary.Id);
        }
        else
        {
            foreach (var properNote in properNotes)
                properNote.UpdateDaySummaryId(existingSummaryForTheDay.Id);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        if (deserializedDaySummary is not null)
        {
            return new GenerateSummarizeDayResult(
                new DaySummarizeResultDTO(
                    command.UserId,
                    deserializedDaySummary.Content,
                    deserializedDaySummary.OriginalContent,
                    (decimal)deserializedDaySummary.HappinessLevel,
                    deserializedDaySummary.PatientGeneralFunctioning,
                    deserializedDaySummary.OriginalPatientGeneralFunctioning,
                    deserializedDaySummary.Interests,
                    deserializedDaySummary.OriginalInterests,
                    deserializedDaySummary.SocialRelationships,
                    deserializedDaySummary.OriginalSocialRelationships,
                    deserializedDaySummary.Work,
                    deserializedDaySummary.OriginalWork,
                    deserializedDaySummary.Family,
                    deserializedDaySummary.OriginalFamily,
                    deserializedDaySummary.PhysicalHealth,
                    deserializedDaySummary.OriginalPhysicalHealth,
                    deserializedDaySummary.Memories,
                    deserializedDaySummary.OriginalMemories,
                    deserializedDaySummary.ReportedProblems,
                    deserializedDaySummary.OriginalReportedProblems,
                    deserializedDaySummary.Other,
                    deserializedDaySummary.OriginalOther
                )
            );
        }

        return new GenerateSummarizeDayResult(
            new DaySummarizeResultDTO(
                command.UserId,
                existingSummaryForTheDay.Content,
                existingSummaryForTheDay.OriginalContent,
                (decimal)existingSummaryForTheDay.HappinessLevel,
                existingSummaryForTheDay.PatientGeneralFunctioning,
                existingSummaryForTheDay.OriginalPatientGeneralFunctioning,
                existingSummaryForTheDay.Interests,
                existingSummaryForTheDay.OriginalInterests,
                existingSummaryForTheDay.SocialRelationships,
                existingSummaryForTheDay.OriginalSocialRelationships,
                existingSummaryForTheDay.Work,
                existingSummaryForTheDay.OriginalWork,
                existingSummaryForTheDay.Family,
                existingSummaryForTheDay.OriginalFamily,
                existingSummaryForTheDay.PhysicalHealth,
                existingSummaryForTheDay.OriginalPhysicalHealth,
                existingSummaryForTheDay.Memories,
                existingSummaryForTheDay.OriginalMemories,
                existingSummaryForTheDay.ReportedProblems,
                existingSummaryForTheDay.OriginalReportedProblems,
                existingSummaryForTheDay.Other,
                existingSummaryForTheDay.OriginalOther
            )
        );
    }
}