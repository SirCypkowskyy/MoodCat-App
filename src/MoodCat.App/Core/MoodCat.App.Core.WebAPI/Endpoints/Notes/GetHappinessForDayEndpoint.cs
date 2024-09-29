using System.Security.Claims;
using Carter;
using MediatR;
using MoodCat.App.Core.Application.Notes.Queries.GetHappinessForDay;

namespace MoodCat.App.Core.WebAPI.Endpoints.Notes;

// /// <summary>
// /// Zapytanie zwracające zadowolenie dla wybranego dnia
// /// </summary>
// /// <param name="Day">
// /// Wybrany dzień
// /// </param>
// public record GetHappinessForDayRequest(DateOnly Day);

/// <summary>
/// Wynik zapytania zwracającego zapytanie dla wybranego dnia
/// </summary>
/// <param name="Happiness"></param>
public record GetHappinessForDayResponse(decimal Happiness);

public class GetHappinessForDayEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/notes/get-day-happiness",
                async (ISender sender, DateTime day, ClaimsPrincipal claimsPrincipal) =>
                {
                    var userId = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                    if (userId is null)
                        throw new ApplicationException("User ID cannot be null.");
                    
                    var result = await sender.Send(new GetHappinessForDayQuery(DateOnly.FromDateTime(day), userId));
                    
                    return new GetHappinessForDayResponse(result.Happiness);
                })
            .RequireAuthorization()
            .Produces<GetHappinessForDayResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("GetHappinessForDay")
            .WithSummary("Returns happiness for the day")
            .WithDescription("Returns happiness for the day")
            .WithDisplayName("Get Happiness for Day")
            .WithTags("Notes");
    }
}