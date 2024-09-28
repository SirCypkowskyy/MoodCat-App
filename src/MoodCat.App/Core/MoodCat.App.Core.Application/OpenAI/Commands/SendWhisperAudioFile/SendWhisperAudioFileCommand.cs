using FluentValidation;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;

namespace MoodCat.App.Core.Application.OpenAI.Commands.SendGptAudioFile;

public record SendWhisperAudioFileCommand(WhisperRequestDTO Request) : ICommand<SendWhisperAudioFileResult>;

public record SendWhisperAudioFileResult(WhisperResultDTO Result);

/// <summary>
/// Walidator żądania do OpenAI Whisper
/// </summary>
public class SendWhisperAudioFileCommandValidator : AbstractValidator<SendWhisperAudioFileCommand>
{
    public SendWhisperAudioFileCommandValidator()
    {
        RuleFor(x => x.Request).NotNull();
        RuleFor(x => x.Request.Model).NotNull().NotEmpty();
        RuleFor(x => x.Request.File).NotNull().NotEmpty();
    }
}