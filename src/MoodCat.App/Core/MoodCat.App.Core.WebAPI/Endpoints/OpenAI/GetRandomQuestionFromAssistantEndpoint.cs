using Carter;
using MediatR;
using MoodCat.App.Core.Application.OpenAI.Queries.GetRandomQuestionForUser;

namespace MoodCat.App.Core.WebAPI.Endpoints.OpenAI;

/// <summary>
/// Zapytanie pozyskania losowego pytania od asystenta AI
/// </summary>
/// <param name="Topic">
/// Temat pytania. Domyślnie `"Content"`, dozwolone wartości:
/// <code>Content, PatientGeneralFunctioning, Interests, SocialRelationships,
/// Work, Family, PhysicalHealth, Memories, ReportedProblems, Other</code>
/// </param>
/// <param name="Tags">
/// Tagi do użycia przy tworzeniu pytania dla użytkownika
/// </param>
/// <param name="Language"></param>
public record GetRandomQuestionFromAssistantRequest(
    string Topic = "Content",
    string Language = "en",
    List<string>? Tags = null
);

/// <summary>
/// Endpoint generujący losowe pytanie od AI (służące jako wzór co omówić w notatce)
/// </summary>
public class GetRandomQuestionFromAssistantEndpoint : ICarterModule
{
    private static readonly string[] _allowedTopics = new string[]
    {
        "Content", "PatientGeneralFunctioning", "Interests", "SocialRelationships", "Work", 
        "Family", "PhysicalHealth",
        "Memories", "ReportedProblems", "Other"
    };

    /// <intheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/openai/generate-question-for-user",
                async (GetRandomQuestionForUserQuery query, ISender sender) =>
                {
                    if (!_allowedTopics.Contains(query.Topic))
                        throw new ArgumentException(
                            $"Invalid topic: {query.Topic}, only valid topics are allowed.\nValid topics: {string.Join(", ", _allowedTopics)}");

                    var result =
                        await sender.Send(new GetRandomQuestionForUserQuery(query.Topic, query.Language, query.Tags));

                    return result;
                })
            .Produces<GetRandomQuestionForUserResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .RequireAuthorization()
            .WithName("GetRandomQuestionFromAssistantEndpoint")
            .WithSummary("Gets random question from assistant")
            .WithDescription("Get random question from assistant for user (in order to complete note)")
            .WithTags("OpenAI");
    }
}