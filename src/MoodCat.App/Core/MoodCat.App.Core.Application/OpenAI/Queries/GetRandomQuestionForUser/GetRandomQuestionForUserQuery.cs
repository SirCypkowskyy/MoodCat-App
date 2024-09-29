using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;

namespace MoodCat.App.Core.Application.OpenAI.Queries.GetRandomQuestionForUser;

/// <summary>
/// Zapytanie generujące losowe pytanie do użytkownika, na podstawie którego mógłby stworzyć notatkę
/// </summary>
/// <param name="Topic">
/// Temat pytania. Domyślnie `"Content"`, dozwolone wartości:
/// <code>Content, PatientGeneralFunctioning, Interests, SocialRelationships,
/// Work, Family, PhysicalHealth, Memories, RepotedProblems, Other</code>
/// </param>
/// <param name="Tags">
/// Tagi do użycia przy tworzeniu pytania dla użytkownika
/// </param>
/// <param name="Language"></param>
public record GetRandomQuestionForUserQuery(
    string Topic = "Content",
    string Language = "en",
    List<string>? Tags = null
) : IQuery<GetRandomQuestionForUserResult>;

/// <summary>
/// Wygenerowane pytanie na podstawie zapytania użytkownika
/// </summary>
/// <param name="Question">
/// Wygenerowane pytanie dla użytkownika
/// </param>
/// <param name="Language">
/// Język, w jakim zostało wygenerowane pytanie dla użytkownika
/// </param>
public record GetRandomQuestionForUserResult(string Question, string Language = "en");