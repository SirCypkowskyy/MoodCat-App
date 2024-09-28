using Carter;
using Microsoft.AspNetCore.Identity;
using MoodCat.App.Core.Domain.Users;

namespace MoodCat.App.Core.WebAPI.Endpoints.Auth;

/// <summary>
/// Endpoint do wylogowywania
/// </summary>
public class LogoutEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/custom/logout", async (SignInManager<User> signInManager) =>
            {
                await signInManager.SignOutAsync();
                return Results.Ok();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithName("Logout")
            .WithSummary("Log out")
            .WithTags("Custom Auth");
    }
}