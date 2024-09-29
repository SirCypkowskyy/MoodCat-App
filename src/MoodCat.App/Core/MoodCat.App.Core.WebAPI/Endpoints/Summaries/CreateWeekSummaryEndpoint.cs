using System.Security.Claims;
using Carter;
using MediatR;
using MoodCat.App.Core.Application.DaySummaries.Commands.GenerateSummarizeDay;

namespace MoodCat.App.Core.WebAPI.Endpoints.Summaries;

/// <summary>
/// Endpoint do tworzenia podsumowań tygodniowych
/// </summary>
public class CreateWeekSummaryEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/day-summaries/generate-summarize-week",
                async (ISender sender, CreateSummaryRequest Request, ClaimsPrincipal claimsPrincipal) =>
                {
                    var userId = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                    if (userId is null)
                        throw new ApplicationException("User ID cannot be null.");

                    throw new NotImplementedException();
                    // var result = await sender.Send(new GenerateSummarizeDayCommand(userId, Request.ForceRefresh));

                    // return result;
                }).WithName("GenerateSummarizeWeekSummaryEndpoint")
            .WithSummary("Generates AI week summary from user notes")
            .WithDescription("Generates a summary of a specific week from user's notes utilising OpenAI's ChatGPT.")
            .RequireAuthorization()
            .Produces<GenerateSummarizeDayResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags("DaySummaries", "Notes", "OpenAI");
    }
}