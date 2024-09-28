namespace MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;

/// <summary>
/// Obiekt transferu danych (DTO) odpowiedzi na żądanie do OpenAI Whisper
/// </summary>
/// <param name="Text">
/// Tekst, jaki wykrył model
/// </param>
public record WhisperResponseDTO(
    string Text
);