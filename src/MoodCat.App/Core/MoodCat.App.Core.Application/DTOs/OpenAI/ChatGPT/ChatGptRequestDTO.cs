namespace MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;

/// <summary>
/// Model transferu danych (DTO) dla requestowania ChatGPT
/// </summary>
/// <param name="model">
/// Wybrany model GPT, np. gpt-3.5-turbo
/// </param>
/// <param name="messages">
/// Wiadomości Requestowane do GPT
/// </param>
public record ChatGptRequestDTO(
    string model,
    string messages
);