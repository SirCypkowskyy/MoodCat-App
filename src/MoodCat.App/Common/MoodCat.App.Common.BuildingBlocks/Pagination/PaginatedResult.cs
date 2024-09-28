namespace MoodCat.App.Common.BuildingBlocks.Pagination;

/// <summary>
/// Klasa reprezentująca wynik paginacji
/// </summary>
/// <param name="pageIndex">
/// Numer strony, którą chcemy pobrać
/// </param>
/// <param name="pageSize">
/// Rozmiar strony, czyli ilość elementów na stronie
/// </param>
/// <param name="count">
/// Całkowita ilość elementów
/// </param>
/// <param name="data">
/// Dane
/// </param>
/// <typeparam name="TEntity">
/// Typ danych, które chcemy zwrócić
/// </typeparam>
public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    where TEntity : class
{
    /// <summary>
    /// Indeks strony
    /// </summary>
    public int PageIndex { get; } = pageIndex;
    
    /// <summary>
    /// Rozmiar strony (ilość elementów na stronie)
    /// </summary>
    public int PageSize { get; } = pageSize;
    
    /// <summary>
    /// Całkowita ilość elementów
    /// </summary>
    public long Count { get; } = count;
    
    /// <summary>
    /// Dane
    /// </summary>
    public IEnumerable<TEntity> Data { get; } = data;
}