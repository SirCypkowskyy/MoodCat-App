using System.Security.Claims;
using Carter;
using MediatR;
using MoodCat.App.Core.Application.Notes.Queries.GetTodaysNotes;

namespace MoodCat.App.Core.WebAPI.Endpoints.Notes;

/// <summary>
/// Endpoint pobierając
/// </summary>
public class GetTodaysNotesEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/notes/today", async (ISender sender, ClaimsPrincipal claims) =>
            {
                var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (userId is null)
                    throw new ApplicationException("User ID cannot be null.");

                var result = await sender.Send(new GetTodaysNotesQuery(userId));

                return result;
            })
            .RequireAuthorization()
            .WithName("GetTodaysNotesEndpoint")
            .WithSummary("Gets Todays Notes")
            .WithDescription(
                "This endpoint is used to get all the notes from the database for authorised user for today.")
            .Produces<GetTodaysNotesResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithTags("Notes");
    }
}