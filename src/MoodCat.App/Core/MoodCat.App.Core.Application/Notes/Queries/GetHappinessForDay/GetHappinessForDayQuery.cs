using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;

namespace MoodCat.App.Core.Application.Notes.Queries.GetHappinessForDay;

/// <summary>
/// Zapytanie zwracające zadowolenie dla wybranego dnia
/// </summary>
/// <param name="Day">
/// Wybrany dzień
/// </param>
public record GetHappinessForDayQuery(DateOnly Day, string UserId) : IQuery<GetHappinessForDayResult>;

/// <summary>
/// Zwraca średnie zadowolenie dla wybranego dnia
/// </summary>
/// <param name="Happiness">
/// Średnie zadowolenie
/// </param>
public record GetHappinessForDayResult(decimal Happiness);