using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;
using MoodCat.App.Core.Application.Interfaces;
using OpenAI.Chat;

namespace MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;

/// <summary>
/// Handler żądania gpt
/// </summary>
/// <param name="clientFactory"></param>
/// <param name="configuration"></param>
/// <param name="logger"></param>
public class SendGptPromptHandler(
    IChatGptService chatGptService
    ) : ICommandHandler<SendGptPromptCommand, SendGptPromptResult>
{
    /// <inheritdoc />
    public async Task<SendGptPromptResult> Handle(SendGptPromptCommand command, CancellationToken cancellationToken)
    {
        if (command.Request is null)
            throw new ApplicationException("Request is null");
        
        var completion = await chatGptService.GenerateResponse(command, cancellationToken: cancellationToken);
        
        var chatGptResponse = new ChatGptResultDTO("", "", "", "", new[]
        {
            new ChatGptChoiceResultDTO("0", "stop",
                new ChatGptResultChoicesMessageDTO("user", completion.Content[0].Text))
        }, new ChatGptResultUsageDTO(0, 0, 0));

        return new SendGptPromptResult(chatGptResponse);
    }
}