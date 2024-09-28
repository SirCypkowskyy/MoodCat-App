using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;
using OpenAI.Chat;

namespace MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;

/// <summary>
/// Handler żądania gpt
/// </summary>
/// <param name="clientFactory"></param>
/// <param name="configuration"></param>
/// <param name="logger"></param>
public class SendGptPromptHandler(
    IHttpClientFactory clientFactory,
    IConfiguration configuration,
    ILogger<SendGptPromptHandler> logger) : ICommandHandler<SendGptPromptCommand, SendGptPromptResult>
{
    public static JsonSerializerOptions SerializeOptions = new JsonSerializerOptions()
    {
        PropertyNameCaseInsensitive = false,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
    };

    /// <inheritdoc />
    public async Task<SendGptPromptResult> Handle(SendGptPromptCommand command, CancellationToken cancellationToken)
    {
        // using var client = clientFactory.CreateClient("OpenAI");

        if (command.Request is null)
            throw new ApplicationException("Request is null");

        // client.BaseAddress = new Uri("https://api.openai.com/v1/chat/");
        // client.DefaultRequestHeaders.Authorization =
        //     new AuthenticationHeaderValue("Bearer", configuration.GetOpenAiApiKey());
        // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


        // using var serializedJson = new StringContent(
        //     JsonSerializer.Serialize(command.Request));

        var chatGptClient = new ChatClient(
            model: command.Request.model,
            configuration.GetOpenAiApiKey()
        );
        logger.LogInformation("Sending GptPrompt request");

        var completion = await chatGptClient.CompleteChatAsync(command.Request.messages);
        logger.LogInformation("Request was successfully sent");


        var chatGptResponse = new ChatGptResultDTO("", "", "", "", new[]
        {
            new ChatGptChoiceResultDTO("0", "stop",
                new ChatGptResultChoicesMessageDTO("user", completion.Value.Content[0].Text))
        }, new ChatGptResultUsageDTO(0, 0, 0));

        return new SendGptPromptResult(chatGptResponse);
    }
}