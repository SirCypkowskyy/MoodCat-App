using FluentValidation;
using MediatR;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;

namespace MoodCat.App.Common.BuildingBlocks.Behaviors;

/// <summary>
/// Klasa obsługująca walidację dla komend i zapytań
/// </summary>
/// <param name="validators">
/// Lista walidatorów do użycia w walidacji
/// </param>
/// <typeparam name="TRequest">
/// Typ zapytania, który ma być obsłużony przez handler
/// </typeparam>
/// <typeparam name="TResponse">
/// Typ wyniku zwracanego przez handler
/// </typeparam>
public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : notnull
{
    /// <inheritdoc/>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        
        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        if (failures.Count != 0)
            throw new ValidationException(failures);
        
        return await next();
    }
}