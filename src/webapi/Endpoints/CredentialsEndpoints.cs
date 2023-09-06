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
        credentials.MapPost("", (JwtTokenService service, LoginRecord blog) =>
        {
            var (username, password) = blog;
            if (username != "camilo" || password != "123") return Results.BadRequest("Username or password is invalid");
            var token = service.GenerateToken(username, "editor");
            return Results.Ok(token);
        });

    }
}