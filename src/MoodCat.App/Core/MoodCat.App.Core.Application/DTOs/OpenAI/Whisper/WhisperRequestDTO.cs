namespace MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;

/// <summary>
/// Obiekt transferu danych (DTO) do żądania do OpenAI Whisper (konwersja głosu na tekst)
/// </summary>
/// <param name="Model">
/// Użyty model
/// </param>
/// <param name="File">
/// Ścieżka do pliku
/// </param>
/// <remarks>
/// DTO musi być użyte jako form (form-data)
/// </remarks>
public record WhisperRequestDTO(
    string Model,
    string File
);