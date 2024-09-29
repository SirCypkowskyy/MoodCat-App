using System.Security.Claims;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoodCat.App.Core.Application.DTOs.Questions;
using MoodCat.App.Core.Application.Notes.Commands.CreateNoteText;
using MoodCat.App.Core.Infrastructure.Data;
using MoodCat.App.Core.WebAPI.Endpoints.Notes;

namespace MoodCat.App.Core.WebAPI.Endpoints.Questions.CompleteQuestion;

/// <summary>
/// Complete question endopoint
/// </summary>
public class CompleteQuestionEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/questions/complete",
                async (long questionId, CreateNoteRequest req, ISender sender, ClaimsPrincipal claimsPrincipal,
                    ApplicationDbContext dbContext) =>
                {
                    var userId = claimsPrincipal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

                    if (userId is null)
                        throw new ApplicationException("User ID cannot be null.");

                    var role = await dbContext.Roles.FirstOrDefaultAsync(x => x.Name == "User");

                    var userRole = await dbContext.UserRoles
                        .FirstOrDefaultAsync(role => role.UserId == userId);

                    if (role!.Id != userRole!.RoleId)
                        throw new UnauthorizedAccessException("You do not have permission to complete questions.");

                    var result = await sender.Send(new CreateNoteTextCommand(req.Data, userId));

                    var question = await dbContext.Questions.FirstOrDefaultAsync(x => x.Id == questionId);

                    question.NoteAnswerId = result.ResponseDataDTO.NoteId;

                    await dbContext.SaveChangesAsync();

                    return new QuestionResponseDTO(question.Id, question.QuestionText, question.TargetUserId,
                        userId, question.IsAnswered);
                })
            .RequireAuthorization()
            .Produces<QuestionResponseDTO>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("CompleteQuestion")
            .WithSummary("Complete question by patient")
            .WithDescription("Complete question by patient")
            .WithTags("Questions");
    }
}