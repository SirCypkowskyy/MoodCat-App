using MediatR;

namespace MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;

/// <summary>
/// Handler zapytania, używany w CQRS
/// </summary>
/// <remarks>
/// Query Handler (Handler zapytania) jest odpowiedzialny za obsługę zapytania i zwrócenie wyniku, zgodnie z zasadami CQRS.
/// </remarks>
/// <typeparam name="TQuery">
/// Typ zapytania, które ma być obsłużone przez handler.
/// </typeparam>
/// <typeparam name="TResponse">
/// Typ wyniku zwracanego przez handler.
/// </typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{
}