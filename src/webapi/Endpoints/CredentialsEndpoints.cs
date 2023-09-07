using System.Runtime.CompilerServices;
using Domain.Enums;
using Domain.Interfaces;

namespace WebApi.Endpoints;

public static partial class EndpointExtensions
{
    public static void MapCredentialsEndpoints(this WebApplication app)
    {
        var credentials = app.MapGroup("/api/credentials/");

        credentials.MapGet("", [Authorize] (ClaimsPrincipal user) =>
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            return new
            {
                Username = username,
                Role = role
            };
        });

        credentials.MapPost("", async (JwtTokenService service,
                                       LoginRecord blog,
                                       IUserService userService,
                                       [EnumeratorCancellation] CancellationToken ct) =>
        {
            var (username, password) = blog;
            var user = await userService.GetAsync(u => u.Username == username,
                                                  ct,
                                                  asNoTracking: true,
                                                  includeNavigationNames: null);
            if (user is null || user?.Password != password) return Results.BadRequest("Username or password is invalid");
            var token = service.GenerateToken(username, Enum.GetName<UserRole>(user.Role));
            return Results.Ok(token);
        });

    }
}