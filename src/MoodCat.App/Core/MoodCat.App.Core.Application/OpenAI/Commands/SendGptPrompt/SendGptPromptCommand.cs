using FluentValidation;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;

namespace MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;

public record SendGptPromptCommand(ChatGptRequestDTO Request) : ICommand<SendGptPromptResult>;

/// <summary />
/// <param name="Result"></param>
public record SendGptPromptResult(ChatGptResultDTO Result);

/// <summary>
/// Walidator żądania do chatGPT
/// </summary>
public class SendGptPromptCommandValidator : AbstractValidator<SendGptPromptCommand>
{
    public SendGptPromptCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();
        RuleFor(x => x.Request.model).NotNull().NotEmpty();
        RuleFor(x => x.Request.messages).NotNull().NotEmpty();
    }
}