using MediatR;

namespace MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;

/// <summary>
/// Interfejs reprezentujący komendę w CQRS.
/// </summary>
/// <remarks>
/// Komenda jest jednym z dwóch rodzajów zapytań w CQRS.
/// Komenda jest zapytaniem, które zmienia stan systemu.
/// Komenda może, ale nie musi zwracać wyniku.
/// </remarks>
public interface ICommand : IRequest<Unit>
{
    
}

/// <summary>
/// Interfejs reprezentujący komendę w CQRS, która zwraca wynik.
/// </summary>
/// <remarks>
/// Komenda jest jednym z dwóch rodzajów zapytań w CQRS.
/// Komenda jest zapytaniem, które zmienia stan systemu.
/// Komenda może, ale nie musi zwracać wyniku.
/// Komenda zwracająca wynik jest używana, gdy chcemy zwrócić wynik z wykonania komendy.
/// </remarks>
/// <typeparam name="TResult">
/// Typ zwracanego wyniku.
/// </typeparam>
public interface ICommand<out TResult> : IRequest<TResult>
{
    
}