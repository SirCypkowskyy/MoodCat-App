using MoodCat.App.Common.BuildingBlocks.Abstractions.CQRS;
using MoodCat.App.Core.Application.Interfaces;

namespace MoodCat.App.Core.Application.OpenAI.Queries.GetRandomQuestionForUser;

/// <summary>
/// Handler obsługujący zapytania stworzenia pytania dla użytkownika
/// </summary>
public class GetRandomQuestionForUserHandler(IChatGptService chatGptService) : IQueryHandler<GetRandomQuestionForUserQuery, GetRandomQuestionForUserResult>
{
    /// <inheritdoc />
    public async Task<GetRandomQuestionForUserResult> Handle(GetRandomQuestionForUserQuery query, CancellationToken cancellationToken)
    {
        var completion = await chatGptService.GenerateQuestionForUser(query, cancellationToken);

        var mergedResponse = string.Join(", ", completion.Content.Select(x => x.Text).ToArray());
        
        return new GetRandomQuestionForUserResult(mergedResponse, query.Language);
    }
}