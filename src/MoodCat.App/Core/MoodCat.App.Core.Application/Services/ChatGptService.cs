using MoodCat.App.Core.Application.Interfaces;
using MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;
using OpenAI.Chat;
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

namespace MoodCat.App.Core.Application.Services;

/// <summary>
/// Provides an easy access to OpenAI ChatGPT services
/// </summary>
public class ChatGptService(
    IConfiguration configuration,
    ILogger<SendGptPromptHandler> logger) : IChatGptService
{
    public async Task<ChatCompletion> GenerateResponse(SendGptPromptCommand command, CancellationToken cancellationToken)
    {
        var chatGptClient = new ChatClient(
            model: command.Request.model,
            configuration.GetOpenAiApiKey()
        );
        logger.LogDebug("Sending a prompt request to OpenAI");

        var completion = await chatGptClient.CompleteChatAsync(command.Request.messages);
        
        logger.LogDebug("Request was successfully sent");
        
        return completion;
    }

    
}