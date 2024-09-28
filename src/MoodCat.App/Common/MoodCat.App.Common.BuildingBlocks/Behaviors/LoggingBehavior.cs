using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MoodCat.App.Common.BuildingBlocks.Behaviors;

/// <summary>
/// Klasa obsługująca logowanie dla komend i zapytań
/// </summary>
/// <param name="logger">
/// Logger do użycia w logowaniu
/// </param>
/// <typeparam name="TRequest">
/// Typ zapytania, który ma być obsłużony przez handler
/// </typeparam>
/// <typeparam name="TResponse">
/// Typ wyniku zwracanego przez handler
/// </typeparam>
public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        logger.LogInformation("[START] Handle request={Request} - Response={Response} - RequestData={RequestData}", 
            typeof(TRequest).Name, typeof(TResponse).Name, request);

        var timer = new Stopwatch();
        timer.Start();
        
        var response = await next();
        
        timer.Stop();
        var timeTaken = timer.Elapsed;
        // Slow performance logging
        if (timeTaken.Seconds > 3)
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken} seconds to process", typeof(TRequest).Name, timeTaken);
        
        logger.LogInformation("[END] Handled {Request} with response {Response} - Time taken: {TimeTaken}", typeof(TRequest).Name, typeof(TResponse).Name, timeTaken);
        
        return response;
    }
}   