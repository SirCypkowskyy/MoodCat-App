namespace MoodCat.App.Core.Application.DTOs.Questions;

/// <summary>
/// Obiekt transferu danych (DTO) do zwracania stworzonych danych
/// </summary>
/// <param name="QuestionId"></param>
/// <param name="Question"></param>
/// <param name="PatientUserId"></param>
/// <param name="SpecialistUserId"></param>
/// <param name="IsAnswered"></param>
public record QuestionResponseDTO(long QuestionId, string Question, string PatientUserId, string SpecialistUserId, bool IsAnswered);