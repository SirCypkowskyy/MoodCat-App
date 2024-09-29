using MediatR;
using MoodCat.App.Core.Application.DTOs.DaySummaries;

namespace MoodCat.App.Core.Application.DaySummaries.Commands.GenerateSummarizeWeek;

/// <summary>
/// Tworzy komendę podsumowania tygodnia
/// </summary>          
/// <param name="UserId">
/// Id użytkownika
/// </param>
/// <param name="ForceRefresh">
/// Wymuś odświeżenie summary jeśli już istnieje
/// </param>
public record GenerateSummarizeWeekCommand(string UserId, bool ForceRefresh) : IRequest<GenerateSummarizeWeekResult>;

/// <summary>
/// Rezultat uruchomienia komendy tworzenia podsumowania tygodnia
/// </summary>
/// <param name="Data">
/// Wygenerowane dane
/// </param>
public record GenerateSummarizeWeekResult(SummarizeResultDTO Data);