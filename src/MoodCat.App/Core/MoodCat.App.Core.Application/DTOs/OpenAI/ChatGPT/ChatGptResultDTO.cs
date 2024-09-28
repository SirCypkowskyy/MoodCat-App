namespace MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;

/// <summary>
/// Obiekt transferu danych (DTO) odpowiedzi na żądanie do OpenAI chatGPT
/// </summary>
/// <param name="Id"></param>
/// <param name="Object"></param>
/// <param name="Created"></param>
/// <param name="Model"></param>
/// <param name="Choices"></param>
/// <param name="Usage"></param>
public record ChatGptResultDTO(
    string Id,
    string Object,
    string Created,
    string Model,
    ChatGptChoiceResultDTO[] Choices,
    ChatGptResultUsageDTO Usage
);