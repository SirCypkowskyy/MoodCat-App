using Carter;
using MediatR;
using Mapster;
using MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;
using MoodCat.App.Core.Application.OpenAI.Commands.SendGptAudioFile;

namespace MoodCat.App.Core.WebAPI.Endpoints.OpenAI;

/// <summary>
/// Request do stworzenia
/// </summary>
/// <param name="Data">
/// Dane do stworzenia transkrypcji
/// </param>
public record CreateWhisperSendAudioFileRequest(
    WhisperRequestDTO Data
);

public record CreateWhisperSendAudioFileResponse(
    WhisperResultDTO Data
);

public class CreateWhisperSendAudioFile : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/openai/whisper", async (CreateWhisperSendAudioFileRequest req, ISender sender,ILogger<CreateWhisperSendAudioFile> logger) =>
            {
                var result = await sender.Send(new SendWhisperAudioFileCommand(req.Data));
                
                logger.LogInformation(result.Result.Result);

                var response = new CreateWhisperSendAudioFileResponse(result.Result);
                
                

                return Results.Ok(response);
            }).WithName("CreateWhisperSendAudioFile")
            .Produces<CreateWhisperSendAudioFileResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Stwórz transkrypcję mowy")
            .WithTags("OpenAI", "WhisperTranscription")
            .WithDescription("Stwórz transkrypcję mowy");
    }
}