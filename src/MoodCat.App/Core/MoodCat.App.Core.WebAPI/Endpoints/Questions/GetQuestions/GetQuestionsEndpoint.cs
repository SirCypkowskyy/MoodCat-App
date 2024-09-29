using System.Security.Claims;
using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoodCat.App.Common.BuildingBlocks.Exceptions;
using MoodCat.App.Core.Infrastructure.Data;

namespace MoodCat.App.Core.WebAPI.Endpoints.Questions.GetQuestions;

/// <summary>
/// Pobiera aktywne pytania użytkownika
/// </summary>
public class GetQuestionsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/questions", async (ISender sender, ClaimsPrincipal claimsPrincipal,
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

                var questions = await dbContext.Questions.Where(x => x.TargetUserId == userId).ToListAsync();

                var result = questions.Select(x => x.QuestionText).ToArray();

                if (result.Length == 0)
                    throw new NotFoundException("Nie znaleziono żadnego aktywnego pytania");

                return Results.Ok(result);
            })
            .RequireAuthorization()
            .Produces<string[]>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("GetQuestionsEndpoint")
            .WithSummary("Get all questions for the user.")
            .WithDescription("Get all questions for the user.")
            .WithTags("Questions");
    }
}