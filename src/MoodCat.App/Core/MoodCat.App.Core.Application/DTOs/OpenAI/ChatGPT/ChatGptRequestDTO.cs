namespace MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;

/// <summary>
/// Model transferu danych (DTO) dla requestowania ChatGPT
/// </summary>
/// <param name="Model">
/// Wybrany model GPT, np. gpt-3.5-turbo
/// </param>
/// <param name="Messages">
/// Wiadomości Requestowane do GPT
/// </param>
public record ChatGptRequestDTO(
    string Model,
    Dictionary<string, string>[] Messages
);