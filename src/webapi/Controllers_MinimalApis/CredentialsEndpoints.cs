namespace WebApi.Endpoints;

public class CredentialEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/credentials");
        group.MapGet("/loggeduser", LoggedUser).WithName(nameof(LoggedUser)).RequireAuthorization("PublicPolicy");
        group.MapPost("/login", Login).WithName(nameof(Login));
        group.MapPost("/register", Register).WithName(nameof(Register));
        group.MapDelete("/delete/{userId}", DeleteWithId).WithName(nameof(DeleteWithId)).RequireAuthorization("EditorPolicy");
        group.MapDelete("/deleteself", DeleteOwnAccount).WithName(nameof(DeleteOwnAccount)).RequireAuthorization("PublicPolicy");
    }

    public static async Task<Result<BlogUser>> DeleteOwnAccount(
        [FromServices] IUserService service,
        ClaimsPrincipal principal)
    {
        var id = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        var result = await service.DeleteAccountWithId(id);
        if (!result.IsSuccess) return Result<BlogUser>.Failure(result.Errors);
        return Result<BlogUser>.Success(result.Value);
    }

    public static async Task<Result<BlogUser>> DeleteWithId(
        [FromServices] IUserService service,
        [FromRoute] string userId)
    {
        var result = await service.DeleteAccountWithId(userId);
        if (!result.IsSuccess) return Result<BlogUser>.Failure(result.Errors);
        return Result<BlogUser>.Success(result.Value);
    }

    public static Result<LoggedUserRecord> LoggedUser(ClaimsPrincipal user)
    {
        return Result<LoggedUserRecord>.Success(new LoggedUserRecord
        (
            Id: user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)!.Value,
            Username: user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value,
            Email: user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value,
            Role: user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value
        ));
    }

    public static async Task<IResult> Login(
                    [FromBody] LoginRecord input,
                    [FromServices] IUserService service)
    {
        var blogUser = await service.GetUserByNameAsync(input.Login);
        if (!blogUser.IsSuccess) return Results.BadRequest(blogUser.Errors);
        var result = await service.LoginAsync(input.Login, input.Password);
        if (!result.IsSuccess) return Results.BadRequest(result.Errors);
        var token = result.Value;
        return Results.Ok(new { token });
    }

    public static async Task<IResult> Register(
                  [FromBody] UserRecordDTO input,
                  [FromServices] IUserService service)
    {
        var blogUser = await service.RegisterAsync(input);
        if (!blogUser.IsSuccess) return Results.BadRequest(blogUser.Errors);
        return Results.Ok(blogUser.Value);
    }
}