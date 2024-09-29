using System.Security.Claims;
using Carter;
using MediatR;
using MoodCat.App.Core.Application.DaySummaries.Commands.GenerateSummarizeDay;

namespace MoodCat.App.Core.WebAPI.Endpoints.Summaries;

/// <summary>
/// Żądanie stworzenia summary dla dnia
/// </summary>
/// <param name="ForceRefresh">
/// Wymuś odświeżenie
/// </param>
public record CreateSummaryRequest(bool ForceRefresh = false);

/// <summary>
/// Endpoint od tworzenia / aktualizacji przez AI podsumowania dnia 
/// </summary>
public class CreateDaySummaryEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/day-summaries/generate-summarize-day",
                async (ISender sender, CreateSummaryRequest Request, ClaimsPrincipal claimsPrincipal) =>
                {
                    var userId = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                    if (userId is null)
                        throw new ApplicationException("User ID cannot be null.");

                    var result = await sender.Send(new GenerateSummarizeDayCommand(userId, Request.ForceRefresh));

                    return result;
                }).WithName("GenerateSummarizeDaySummaryEndpoint")
            .WithSummary("Generates AI day summary from user notes")
            .WithDescription("Generates a summary of a specific day from user's notes utilising OpenAI's ChatGPT.")
            .RequireAuthorization()
            .Produces<GenerateSummarizeDayResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags("DaySummaries", "Notes", "OpenAI");
    }
}