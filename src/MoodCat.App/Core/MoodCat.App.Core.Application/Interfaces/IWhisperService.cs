using MoodCat.App.Core.Application.OpenAI.Commands.SendGptAudioFile;
using OpenAI.Audio;

namespace MoodCat.App.Core.Application.Interfaces;

public interface IWhisperService
{
    public Task<AudioTranscription> TranscribeAudio(SendWhisperAudioFileCommand command, CancellationToken cancellationToken);
}