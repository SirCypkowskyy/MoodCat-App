namespace MoodCat.App.Common.BuildingBlocks.Pagination;

/// <summary>
/// Klasa reprezentująca dane żądania paginacji
/// </summary>
/// <param name="PageIndex">
/// Numer strony, którą chcemy pobrać
/// </param>
/// <param name="PageSize">     
/// Rozmiar strony, czyli ilość elementów na stronie
/// </param>
public record PaginationRequest(int PageIndex = 0, int PageSize = 10);