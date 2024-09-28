using System.Security.Claims;
using Carter;
using MediatR;
using MoodCat.App.Core.Application.DTOs.Notes;
using MoodCat.App.Core.Application.Notes.Queries.GetNoteById;

namespace MoodCat.App.Core.WebAPI.Endpoints.Notes;

/// <summary>
/// Zwraca notatkę po id
/// </summary>
public class GetNoteEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/notes", async (Guid noteId, ISender sender, ClaimsPrincipal claims) =>
            {
                var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (userId is null)
                    throw new ApplicationException("User ID cannot be null.");
                
                var result = await sender.Send(new GetNoteByIdQuery(noteId, userId));

                return Results.Ok(result.Data);
            })
            .WithName("GetNoteEndpoint")
            .RequireAuthorization()
            .WithSummary("Get note by id")
            .WithDescription("Returns notes with provided ID")
            .Produces<NoteDetailedResponseDTO>(StatusCodes.Status200OK)
            .WithTags("Notes");
    }
}