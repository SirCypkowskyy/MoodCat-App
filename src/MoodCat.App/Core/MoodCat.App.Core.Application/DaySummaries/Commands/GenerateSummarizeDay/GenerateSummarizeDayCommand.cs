using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.DTOs.DaySummaries;

namespace MoodCat.App.Core.Application.DaySummaries.Commands.GenerateSummarizeDay;

/// <summary>
/// Tworzy komendę podsumowania dnia
/// </summary>          
/// <param name="UserId">
/// Id użytkownika
/// </param>
/// <param name="ForceRefresh">
/// Wymuś odświeżenie summary jeśli już istnieje
/// </param>
public record GenerateSummarizeDayCommand(string UserId, bool ForceRefresh) : ICommand<GenerateSummarizeDayResult>;

/// <summary>
/// Rezultat uruchomienia komendy tworzenia podsumowania dnia
/// </summary>
/// <param name="Data">
/// Wygenerowane dane
/// </param>
public record GenerateSummarizeDayResult(SummarizeResultDTO Data);