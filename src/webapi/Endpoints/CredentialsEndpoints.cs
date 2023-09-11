using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Endpoints;

public class CredentialEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/credentials/");
        app.MapGet("loggeduser", LoggedUser).WithName(nameof(LoggedUser)).RequireAuthorization();
        app.MapPost("login", Login).WithName(nameof(Login));
        app.MapPost("register", Register).WithName(nameof(Register));
    }

    public static IResult LoggedUser(ClaimsPrincipal user) => TypedResults.Ok(user);

    public static async Task<IResult> Login(
                    [FromBody] LoginRecord input,
                    [FromServices] IUserService service)
    {
        var blogUser = await service.GetUserByNameAsync(input.Login);
        if(!blogUser.IsSuccess) return Results.BadRequest(blogUser.Errors);
        var result = await service.LoginAsync(input.Login, input.Password);
        if (!result.IsSuccess) return Results.BadRequest(result.Errors);
        return Results.Ok(blogUser);
    }

    public static async Task<IResult> Register(
                  [FromBody] UserRecord input,
                  [FromServices] IUserService service)
    {
        var blogUser = await service.RegisterAsync(input);
        if(!blogUser.IsSuccess) return Results.BadRequest(blogUser.Errors);
        return Results.Ok(blogUser.Value);
    }
}

