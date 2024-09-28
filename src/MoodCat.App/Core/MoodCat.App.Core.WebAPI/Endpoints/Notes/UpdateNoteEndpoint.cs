using System.Security.Claims;
using Carter;
using MediatR;
using MoodCat.App.Core.Application.DTOs.Notes;
using MoodCat.App.Core.Application.Notes.Commands.CreateNoteAudio;
using MoodCat.App.Core.Application.Notes.Commands.CreateNoteText;
using MoodCat.App.Core.Application.Notes.Commands.UpdateNote;

namespace MoodCat.App.Core.WebAPI.Endpoints.Notes;

/// <summary>
/// Request aktualizacji notatki
/// </summary>
/// <param name="NoteId"></param>
/// <param name="Data"></param>
public record UpdateNoteRequest(Guid NoteId, CreateNoteRequestDTO Data);

/// <summary>
/// Response aktualizacji notatki
/// </summary>
/// <param name="Response"></param>
public record UpdateNoteResponse(CreateNoteResponseDTO Response);

/// <summary>
/// Endpoint od aktualizacji notatek
/// </summary>
public class UpdateNoteEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/notes/update", async (UpdateNoteRequest req, ISender sender, ClaimsPrincipal claims) =>
            {
                var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (userId is null)
                    throw new ApplicationException("User ID cannot be null.");

                if (
                    (string.IsNullOrWhiteSpace(req.Data.AudioUrl) && string.IsNullOrWhiteSpace(req.Data.Text))
                    || (!string.IsNullOrWhiteSpace(req.Data.AudioUrl) && !string.IsNullOrWhiteSpace(req.Data.Text)))
                    throw new ApplicationException(
                        "Request aktualizacji notatki musi mieć podane audio url LUB tekst, nie oba naraz lub brak żadnego!");

                var result = await sender.Send(new UpdateNoteCommand(req.NoteId, req.Data, userId));

                return new CreateNoteResponse(result.ResponseDataDTO);
            })
            .RequireAuthorization()
            .WithName("Update Note")
            .Produces<CreateNoteResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Update Note with Text or Audio")
            .WithTags("Notes");
    }
}