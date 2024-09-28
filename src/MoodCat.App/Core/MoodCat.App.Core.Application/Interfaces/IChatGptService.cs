using MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;
using OpenAI.Chat;

namespace MoodCat.App.Core.Application.Interfaces;

public interface IChatGptService
{
    public Task<ChatCompletion> GenerateResponse(SendGptPromptCommand command, CancellationToken cancellationToken);

}