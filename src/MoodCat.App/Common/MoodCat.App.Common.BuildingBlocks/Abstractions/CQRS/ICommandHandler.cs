using MediatR;

namespace MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;

/// <summary>
/// Handler komendy bez zwracania wyniku, używany w CQRS
/// </summary>
/// <remarks>
/// W przypadku, gdy komenda ma zwracać wynik, należy użyć interfejsu <see cref="ICommandHandler{TCommand, TResult}"/>.
/// </remarks>
/// <typeparam name="TCommand">
/// Typ komendy, która ma być obsłużona przez handler.
/// </typeparam>
public interface ICommandHandler<in TCommand>
: IRequestHandler<TCommand, Unit>
where TCommand : ICommand
{
    
}


/// <summary>
/// Handler komendy z zwracaniem wyniku, używany w CQRS
/// </summary>
/// <remarks>
/// W przypadku, gdy komenda nie ma zwracać wyniku, należy użyć interfejsu <see cref="ICommandHandler{TCommand}"/>.
/// </remarks>
/// <typeparam name="TCommand">
/// Typ komendy, która ma być obsłużona przez handler.
/// </typeparam>
/// <typeparam name="TResponse">
/// Typ wyniku zwracanego przez handler.
/// </typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}