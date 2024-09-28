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
/// <param name="RequestData">
/// Dane żądania
/// </param>
public record CreateNoteTextRequest(CreateNoteTextRequestDTO RequestData);

/// <summary>
/// Notatka do stworzenia
/// </summary>
/// <param name="AudioUrl">
/// Url do pliku audio, który ma zostać użyty do stworzenia notatki
/// </param>
public record CreateNoteAudioRequest(string NoteTitle, string AudioUrl);

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
        app.MapPost("/api/notes/create", async (CreateNoteTextRequest req, ISender sender, ClaimsPrincipal claims) =>
            {
                var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                var result = await sender.Send(new CreateNoteTextCommand(req.RequestData, userId));

                return new CreateNoteResponse(result.ResponseDataDTO);
            })
            .RequireAuthorization()
            .WithName("Create Note")
            .Produces<CreateNoteResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Note with Text")
            .WithTags("Notes");

        app.MapPost("/api/notes/create-audio",
                async (CreateNoteAudioRequest req, ISender sender, ClaimsPrincipal claims) =>
                {
                    var userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                    var result = await sender.Send(new CreateNoteAudioCommand(req.NoteTitle, req.AudioUrl, userId));

                    return new CreateNoteResponse(result.ResponseDataDTO);
                })
            .RequireAuthorization()
            .WithName("Create Note Audio")
            .Produces<CreateNoteResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Note with Audio Url")
            .WithTags("Notes");
    }
}