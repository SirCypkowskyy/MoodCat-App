using MoodCat.App.Core.Application.Interfaces;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;
using MoodCat.App.Core.Application.OpenAI.Commands.SendGptAudioFile;
using OpenAI.Audio;

namespace MoodCat.App.Core.Application.Services;

/// <summary>
/// Provides an easy access to OpenAI Whisper services
/// </summary>
public class WhisperService(
    IConfiguration configuration,
    ILogger<WhisperService> logger) : IWhisperService
{
    public async Task<AudioTranscription> TranscribeAudio(SendWhisperAudioFileCommand command,
        CancellationToken cancellationToken)
    {
        var whisperClient = new AudioClient(
            model: "whisper-1",
            configuration.GetOpenAiApiKey());

        var audioFilePath = command.Request.File;
        byte[] audiofileBytes;
        var httpClient = new HttpClient();
        using (HttpResponseMessage response = await httpClient.GetAsync(audioFilePath, cancellationToken))
        {
            response.EnsureSuccessStatusCode();
            audiofileBytes = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        }

        var audioTranscriptionOptions = new AudioTranscriptionOptions()
        {
            ResponseFormat = AudioTranscriptionFormat.Text
        };

        using (MemoryStream stream = new MemoryStream(audiofileBytes))
        {
            var transcription = await whisperClient.TranscribeAudioAsync(stream, "transcribe.mp3",
                options: audioTranscriptionOptions, cancellationToken: cancellationToken);


            logger.LogDebug("Sending whisper request.");

            logger.LogDebug($"Transcribed result: {transcription.Value.Text}");

            return transcription;
        }
    }
}