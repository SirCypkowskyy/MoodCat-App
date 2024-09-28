namespace MoodCat.App.Core.Application.DTOs.OpenAI.ChatGPT;

/// <summary>
/// Model transferu danych dla wiadomości w ramach wyboru
/// </summary>
/// <param name="Role"></param>
/// <param name="Content"></param>
public record ChatGptResultChoicesMessageDTO(
    string Role,
    string Content
);