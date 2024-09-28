using MediatR;

namespace MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;

/// <summary>
/// Interfejs zapytania, używany w CQRS
/// </summary>
/// <remarks>
/// Query (zapytanie) jest operacją, która zwraca wynik, ale nie zmienia/nie powinna zmieniać stanu systemu.
/// Każde zapytanie powinno implementować ten interfejs. Zapytanie zawsze powinno zwracać jakiś wynik.
/// </remarks>
/// <typeparam name="TResponse">
/// Typ wyniku zapytania.
/// </typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}