using Carter;
using MediatR;
using MoodCat.App.Common.BuildingBlocks.Pagination;
using MoodCat.App.Core.Application.DTOs.Users;

namespace MoodCat.App.Core.WebAPI.Endpoints.Users;

/// <summary>
/// Odpowiedź na żądanie pobrania paginowanej listy użytkowników
/// </summary>
/// <param name="Users"></param>
public record GetUsersResponse(PaginatedResult<UserResponseDTO> Users);

public class GetUsersPaginatedEndpoint : ICarterModule
{
    /// <inheritdoc />
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users",
                async ([AsParameters] PaginationRequest request, ISender sender) =>
                {
                    throw new NotImplementedException();
                })
            .WithName("GetUsersPaginated")
            .Produces<GetUsersResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithSummary("Get users paginated")
            .WithDescription("Get users paginated")
            .RequireAuthorization(e => { e.RequireRole("Admin"); })
            .WithTags("Users");
    }
}