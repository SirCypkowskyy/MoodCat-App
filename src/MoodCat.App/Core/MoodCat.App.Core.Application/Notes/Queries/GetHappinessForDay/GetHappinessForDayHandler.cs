using Microsoft.EntityFrameworkCore;
using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.Application.Notes.Queries.GetHappinessForDay;

/// <summary>
/// Handler obsługujący zapytanie zwracające średnie zadowolenie na wybrany dzień
/// </summary>
public class GetHappinessForDayHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetHappinessForDayQuery, GetHappinessForDayResult>
{
    /// <inheritdoc />
    public async Task<GetHappinessForDayResult> Handle(GetHappinessForDayQuery request,
        CancellationToken cancellationToken)
    {
        var summaryForTheDay = await dbContext.DaysSummaries
            .Include(x => x.Notes)
            .FirstOrDefaultAsync(
                x => x.UserId == request.UserId && DateOnly.FromDateTime(x.CreatedAt!.Value) == request.Day,
                cancellationToken);
        
        if(summaryForTheDay != null)
            return new GetHappinessForDayResult((decimal)summaryForTheDay.HappinessLevel);

        var noteRecordsFromDay = await dbContext.Notes
            .Where(x => x.UserId == request.UserId && DateOnly.FromDateTime(x.CreatedAt!.Value) == request.Day)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        if (noteRecordsFromDay.Count == 0 || !noteRecordsFromDay.Any(x => x.Happiness is not null && x.Happiness > 0))
            return new GetHappinessForDayResult(0);

        var result = noteRecordsFromDay.Select(x => x.Happiness).Average();
        return new GetHappinessForDayResult((decimal)result!);
    }
}