using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;
using MoodCat.App.Core.Application.Interfaces;
using OpenAI.Audio;
namespace MoodCat.App.Core.Application.OpenAI.Commands.SendWhisperAudioFile;

/// <summary>
/// Handler żądania OpenAI Whisper
/// </summary>
public class SendWhisperAudioFileHandler(IWhisperService whisperService)
    : ICommandHandler<SendWhisperAudioFileCommand, SendWhisperAudioFileResult>
{
    /// <inheritdoc />
    public async Task<SendWhisperAudioFileResult> Handle(SendWhisperAudioFileCommand command,
        CancellationToken cancellationToken)
    {
        if (command.Request is null)
        {
            throw new ApplicationException("Request is null");
        }

        var transcription = await whisperService.TranscribeAudio(command, cancellationToken);

        var whisperResponse = new WhisperResultDTO("", "", "", "whisper-1", transcription.Text);

        return new SendWhisperAudioFileResult(whisperResponse);
        
    }
}