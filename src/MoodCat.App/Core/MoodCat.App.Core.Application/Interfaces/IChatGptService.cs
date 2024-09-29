using MoodCat.App.Core.Application.OpenAI.Commands.SendGptPrompt;
using MoodCat.App.Core.Application.OpenAI.Queries.GetRandomQuestionForUser;
using OpenAI.Chat;

namespace MoodCat.App.Core.Application.Interfaces;

/// <summary>
/// Interfejs obsługi ChatGPT
/// </summary>
public interface IChatGptService
{
    /// <summary>
    /// Generuje odpowiedź chatgpt na żądanie stworzenia podsumowania dnia
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ChatCompletion> GenerateResponse(SendGptPromptCommand command, CancellationToken cancellationToken);

    /// <summary>
    /// Generuje pytanie dla użytkownika na podstawie podanych ustawień
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<ChatCompletion> GenerateQuestionForUser(GetRandomQuestionForUserQuery query, CancellationToken cancellationToken);
}