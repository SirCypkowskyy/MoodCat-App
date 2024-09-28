using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;

namespace MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;

/// <summary>
/// Handler żądania gpt
/// </summary>
/// <param name="clientFactory"></param>
/// <param name="configuration"></param>
/// <param name="logger"></param>
public class SendGptPromptHandler(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<SendGptPromptHandler> logger) : ICommandHandler<SendGptPromptCommand, SendGptPromptResult>
{
    /// <inheritdoc />
    public async Task<SendGptPromptResult> Handle(SendGptPromptCommand command, CancellationToken cancellationToken)
    {
        using var client = clientFactory.CreateClient("OpenAI");
        
        client.BaseAddress = new Uri("https://api.openai.com/v1/chat/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.GetOpenAiApiKey());
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        using var serializedJson = new StringContent(
            JsonSerializer.Serialize(command.Request)
            , Encoding.UTF8
            , "application/json");
        
        logger.LogInformation("Sending GptPrompt request");
        logger.LogDebug("GptPrompt request: {request}", JsonSerializer.Serialize(command.Request));
        using var response = await client.PostAsJsonAsync("completions", serializedJson, cancellationToken);

        response.EnsureSuccessStatusCode();
        logger.LogInformation("Request was successfully sent");
        
        var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogDebug("GptPrompt response: {response}", jsonResponse);

        var deserializedJson = JsonSerializer.Deserialize<ChatGptResultDTO>(jsonResponse)!;
        
        return new SendGptPromptResult(deserializedJson);
    }
}