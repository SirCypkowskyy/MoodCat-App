using System.Security.Claims;
using Azure.Core;
using Carter;
using MediatR;
using MoodCat.App.Core.Application.DaySummaries.Commands.GenerateSummarizeDay;

namespace MoodCat.App.Core.WebAPI.Endpoints.DaySummaries;


/// <summary>
/// Żądanie stworzenia summary dla dnia
/// </summary>
/// <param name="ForceRefresh">
/// Wymuś odświeżenie
/// </param>
public record CreateDaySummaryRequest(bool ForceRefresh = false);

/// <summary>
/// Endpoint od tworzenia / aktualizacji przez AI podsumowania dnia 
/// </summary>
public class CreateDaySummaryEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/day-summaries/generate-summarize-day", async (ISender sender, CreateDaySummaryRequest Request, ClaimsPrincipal claimsPrincipal) =>
        {
            var userId = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

            if (userId is null)
                throw new ApplicationException("User ID cannot be null.");

            var result = await sender.Send(new GenerateSummarizeDayCommand(userId, Request.ForceRefresh));
            
            return result;
            
        }).WithName("GenerateSummarizeDaySummaryEndpoint")
        .RequireAuthorization()
        .ProducesProblem(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status500InternalServerError);
    }
}