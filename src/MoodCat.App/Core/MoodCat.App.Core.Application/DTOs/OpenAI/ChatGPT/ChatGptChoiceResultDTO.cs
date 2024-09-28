namespace MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;

/// <summary>
/// Obiekt transferu danych (DTO) dla wyborów w outputcie ChatGPT
/// </summary>
/// <param name="Index"></param>
/// <param name="Finish_Reason"></param>
/// <param name="Message"></param>
public record ChatGptChoiceResultDTO(
    string Index,
    string Finish_Reason,
    ChatGptResultChoicesMessageDTO Message
);