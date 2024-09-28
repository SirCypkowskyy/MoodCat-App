namespace MoodCat.App.Core.Application.DTOs.OpenAI.Whisper;

/// <summary>
/// Obiekt transfery danych (DTO) odpowiedzi na żądanie do OpenAI Whisper
/// </summary>
/// <param name="Id"></param>
/// <param name="Object"></param>
/// <param name="Created"></param>
/// <param name="Model"></param>
public class WhisperResultDTO(
    string Id, 
    string Object, 
    string Created, 
    string Model,
    string Result
);