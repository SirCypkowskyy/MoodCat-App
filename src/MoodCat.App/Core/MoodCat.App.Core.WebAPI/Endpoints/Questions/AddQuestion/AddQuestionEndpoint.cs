using System.Security.Claims;
using Carter;
using Microsoft.EntityFrameworkCore;
using MoodCat.App.Core.Application.DTOs.Questions;
using MoodCat.App.Core.Domain.Questions;
using MoodCat.App.Core.Infrastructure.Data;
using MoodCat.Core.Application.Data;

namespace MoodCat.App.Core.WebAPI.Endpoints.Questions.AddQuestion;

/// <summary>
/// Add question request
/// </summary>
/// <param name="TargetUserId"></param>
/// <param name="QuestionContent"></param>
public record AddQuestionRequest(
    string TargetUserId,
    string QuestionContent
);

/// <summary>
/// Dodaje nowe pytanie
/// </summary>
public class AddQuestionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/questions/add",
                async (AddQuestionRequest request, ApplicationDbContext dbContext, ClaimsPrincipal claimsPrincipal) =>
                {
                    var userId = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                    if (userId is null)
                        throw new ApplicationException("User ID cannot be null.");

                    var role = await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == "Specialist");
                    
                    var userRole = await dbContext.UserRoles
                        .FirstOrDefaultAsync(role => role.UserId == userId);

                    if (role!.Id != userRole!.RoleId)
                        throw new UnauthorizedAccessException("You do not have permission to add this question.");

                    var newQuestion = new QuestionEntity()
                    {
                        TargetUserId = request.TargetUserId,
                        QuestionText = request.QuestionContent,
                        SpecialistUserId = userId
                    };

                    await dbContext.Questions.AddAsync(newQuestion);

                    await dbContext.SaveChangesAsync();

                    return new QuestionResponseDTO(newQuestion.Id, newQuestion.QuestionText, newQuestion.TargetUserId,
                        userId, newQuestion.IsAnswered);
                })
            .RequireAuthorization()
            .Produces<QuestionResponseDTO>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("AddQuestion")
            .WithSummary("Adds a question to a patient by specialist")
            .WithDescription("Adds a question to a patient by specialist")
            .WithTags("Questions");
    }
}