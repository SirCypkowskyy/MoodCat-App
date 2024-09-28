namespace MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;

/// <summary>
/// Obiekt transferu danych (DTO) dla wyników użytych tokenów w odpowiedzi
/// </summary>
/// <param name="Prompt_tokens"></param>
/// <param name="Completion_tokens"></param>
/// <param name="Total_tokens"></param>
public record ChatGptResultUsageDTO(
    int Prompt_tokens,
    int Completion_tokens,
    int Total_tokens
);