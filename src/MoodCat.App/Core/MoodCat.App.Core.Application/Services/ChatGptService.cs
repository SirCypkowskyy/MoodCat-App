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
using MoodCat.App.Core.Application.OpenAI.Constants;
using MoodCat.App.Core.Application.OpenAI.Queries.GetRandomQuestionForUser;
using OpenAI.Chat;

namespace MoodCat.App.Core.Application.Services;

/// <summary>
/// Provides easy access to OpenAI ChatGPT services
/// </summary>
public class ChatGptService(
    IConfiguration configuration,
    ILogger<SendGptPromptHandler> logger) : IChatGptService
{
    /// <inheritdoc />
    public async Task<ChatCompletion> GenerateResponse(SendGptPromptCommand command,
        CancellationToken cancellationToken)
    {
        var chatGptClient = new ChatClient(
            model: command.Request.Model,
            configuration.GetOpenAiApiKey()
        );
        logger.LogDebug("Sending a prompt request to OpenAI");

        var messages = new List<ChatMessage>
        {
            ChatMessage.CreateSystemMessage(OpenAIConstants.SystemInstructionsForGeneratingSummary),
            ChatMessage.CreateUserMessage(command.Request.Message),
        };

        var completion = await chatGptClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);

        logger.LogDebug("Request was successfully sent");

        return completion;
    }

    /// <inheritdoc />
    public async Task<ChatCompletion> GenerateQuestionForUser(GetRandomQuestionForUserQuery query,
        CancellationToken cancellationToken)
    {
        var chatGptClient = new ChatClient(
            model: configuration.GetOpenAiGptModel(),
            configuration.GetOpenAiApiKey()
        );
        logger.LogDebug("Sending a prompt request to OpenAI");

        var messages = new List<ChatMessage>();
        if (query.Tags is { Count: > 0 })
        {
            var tags = string.Join(", ", query.Tags.Select(x => "\"" + x + "\"").ToArray());

            messages.AddRange([
                ChatMessage.CreateSystemMessage(OpenAIConstants.SystemInstructionsForGeneratingQuestionForUser),
                ChatMessage.CreateUserMessage($"**Input**:" +
                                              $"\n- Topic: \"{query.Topic}\"" +
                                              $"\n- Language: \"{query.Language}\"" +
                                              $"\n- Tags: [{tags}]")
            ]);
        }
        else
        {
            messages.AddRange([
                ChatMessage.CreateSystemMessage(OpenAIConstants.SystemInstructionsForGeneratingQuestionForUser),
                ChatMessage.CreateUserMessage($"**Input**:\n- Topic: \"{query.Topic}\"\n" +
                                              $"- Language: \"{query.Language}\"")
            ]);
        }


        var completion = await chatGptClient.CompleteChatAsync(messages, cancellationToken: cancellationToken);

        logger.LogDebug("Request was successfully sent");

        return completion;
    }
}