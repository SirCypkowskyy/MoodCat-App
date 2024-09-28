using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Common.BuildingBlocks.Extensions;
using MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;
using MoodCat.App.Core.Application.OpenAI.Commands.SendGptAudioFile;
using OpenAI.Audio;

namespace MoodCat.App.Core.Application.OpenAI.Commands.SendWhisperAudioFile;

/// <summary>
/// Handler żądania OpenAI Whisper
/// </summary>
public class SendWhisperAudioFileHandler(
    IHttpClientFactory clientFactory,
    IConfiguration configuraiton,
    ILogger<SendWhisperAudioFileHandler> logger)
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

        var whisperClient = new AudioClient(
            model: "whisper-1",
            configuraiton.GetOpenAiApiKey());

        var audioFilePath = "";
        byte[] audiofileBytes;
        var httpClient = new HttpClient();
        using (HttpResponseMessage response = await httpClient.GetAsync(audioFilePath))
        {
            response.EnsureSuccessStatusCode();
            audiofileBytes = await response.Content.ReadAsByteArrayAsync();
        }

        var audioTranscriptionOptions = new AudioTranscriptionOptions()
        {
            ResponseFormat = AudioTranscriptionFormat.Text,
        };

        using (MemoryStream stream = new MemoryStream(audiofileBytes))
        {
            var transcription = await whisperClient.TranscribeAudioAsync(stream, "transcribe.mp3", options: audioTranscriptionOptions);


            logger.LogInformation("Sending whisper request.");

            logger.LogInformation($"Transcripted result: {transcription.Value.Text}");

            var whisperResponse = new WhisperResultDTO("", "", "", "whisper-1", transcription.Value.Text);

            return new SendWhisperAudioFileResult(whisperResponse);
        }
    }
}