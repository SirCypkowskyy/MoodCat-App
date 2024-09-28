using Azure.Core;
using Carter;
using Mapster;
using MediatR;
using MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;
using MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;

namespace MoodCat.App.Core.WebAPI.Endpoints.OpenAI;

/// <summary>
/// Request do stworzenia 
/// </summary>
/// <param name="Data">
/// Dane do stworzenia completion
/// </param>
public record CreateGptChatCompletionRequest(
    ChatGptRequestDTO Data
);

public record CreateGptChatCompletionResponse(
    ChatGptResultDTO Data
);

public class CreateGptChatCompletion : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/openai/chatgpt", async (CreateGptChatCompletionRequest req, ISender sender) =>
            {
                
                var result = await sender.Send(new SendGptPromptCommand(req.Data));

                var response = new CreateGptChatCompletionResponse(result.Result);

                return Results.Ok(response);
            }).WithName("CreateGptChatCompletion")
            .Produces<CreateGptChatCompletionResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Wygeneruj request do chatgpt")
            .WithTags("OpenAI", "GptChatCompletion")
            .WithDescription("Wygeneruj request do chatgpt");
    }
}