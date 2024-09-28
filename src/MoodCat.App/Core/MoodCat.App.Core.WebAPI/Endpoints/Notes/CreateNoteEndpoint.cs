using System.Security.Claims;
using Carter;
using MediatR;
using MoodCat.App.Core.Application.DTOs.Notes;
using MoodCat.App.Core.Application.Notes.Commands.CreateNoteAudio;
using MoodCat.App.Core.Application.Notes.Commands.CreateNoteText;

namespace MoodCat.App.Core.WebAPI.Endpoints.Notes;

/// <summary>
/// Notatka do stworzenia
/// </summary>
/// <param name="Data">
/// Dane żądania
/// </param>
public record CreateNoteRequest(CreateNoteRequestDTO Data);

/// <summary>
/// Odpowiedź na żądanie
/// </summary>
/// <param name="Response">
/// Odpowiedź
/// </param>
public record CreateNoteResponse(CreateNoteResponseDTO Response);

/// <summary>
/// Create note endpoint
/// </summary>
public class CreateNoteEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/notes/create", async (CreateNoteRequest req, ISender sender, ClaimsPrincipal claims) =>
            {
                var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                if (userId is null)
                    throw new ApplicationException("User ID cannot be null.");

                if (
                    (string.IsNullOrWhiteSpace(req.Data.AudioUrl) && string.IsNullOrWhiteSpace(req.Data.Text))
                    || (!string.IsNullOrWhiteSpace(req.Data.AudioUrl) && !string.IsNullOrWhiteSpace(req.Data.Text)))
                    throw new ApplicationException(
                        "Request stworzenia notatki musi mieć podane audio url LUB tekst, nie oba naraz lub brak żadnego!");

                if (string.IsNullOrWhiteSpace(req.Data.AudioUrl))
                {
                    var textResult = await sender.Send(new CreateNoteTextCommand(req.Data, userId));

                    return new CreateNoteResponse(
                        new CreateNoteResponseDTO(
                            textResult.ResponseDataDTO.NoteId,
                            textResult.ResponseDataDTO.Title,
                            textResult.ResponseDataDTO.Content)
                    );
                }

                var result = await sender.Send(
                    new CreateNoteAudioCommand(
                        req.Data.Title,
                        req.Data.AudioUrl,
                        userId,
                        req.Data.Meta.HappinessLevel,
                        req.Data.Meta.ProvidedQuestion
                    )
                );

                return new CreateNoteResponse(result.ResponseDataDTO);
            })
            .RequireAuthorization()
            .WithName("Create Note")
            .Produces<CreateNoteResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Note with Text or Audio")
            .WithTags("Notes");
    }
}