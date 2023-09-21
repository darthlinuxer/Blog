namespace WebApi.Endpoints;

public class CredentialEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/credentials");

        //Unprotected Endpoints
        group.MapPost("/login", Login).WithName(nameof(Login));
        group.MapPost("/register", Register).WithName(nameof(Register));

        //Authorized as Public
        group.MapGet("/loggeduser", LoggedUser).WithName(nameof(LoggedUser)).RequireAuthorization("PublicPolicy");
        group.MapDelete("/deleteself", DeleteOwnAccount).WithName(nameof(DeleteOwnAccount)).RequireAuthorization("PublicPolicy");

        //Editor Endpoints
        group.MapGet("/getbyid/{userId}", GetById).WithName(nameof(GetById)).RequireAuthorization("EditorPolicy");
        group.MapGet("/getbyname/{user}", GetByName).WithName(nameof(GetByName)).RequireAuthorization("EditorPolicy");
        group.MapGet("/getbyemail/{email}", GetByEmail).WithName(nameof(GetByEmail)).RequireAuthorization("EditorPolicy");
        group.MapDelete("/delete/{userId}", DeleteWithId).WithName(nameof(DeleteWithId)).RequireAuthorization("EditorPolicy");
    }

    public static async Task<Result<Person>> GetById(
        string userId,
        [FromServices] IPersonService service)
    {
        var result = await service.GetUserByIdAsync(userId);
        if (!result.IsSuccess) return Result<Person>.Failure(result.Errors);
        return Result<Person>.Success(result.Value);
    }

    public static async Task<Result<Person>> GetByName(
        string user,
        [FromServices] IPersonService service)
    {
        var result = await service.GetUserByNameAsync(user);
        if (!result.IsSuccess) return Result<Person>.Failure(result.Errors);
        return Result<Person>.Success(result.Value);
    }

    public static async Task<Result<Person>> GetByEmail(
       string email,
       [FromServices] IPersonService service)
    {
        var result = await service.GetUserByEmailAsync(email);
        if (!result.IsSuccess) return Result<Person>.Failure(result.Errors);
        return Result<Person>.Success(result.Value);
    }

    public static async Task<Result<Person>> DeleteOwnAccount(
        [FromServices] IPersonService service,
        ClaimsPrincipal principal)
    {
        var id = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;
        var result = await service.DeleteAccountWithId(id!);
        if (!result.IsSuccess) return Result<Person>.Failure(result.Errors);
        return Result<Person>.Success(result.Value);
    }

    public static async Task<Result<Person>> DeleteWithId(
        [FromServices] IPersonService service,
        [FromRoute] string userId)
    {
        var result = await service.DeleteAccountWithId(userId);
        if (!result.IsSuccess) return Result<Person>.Failure(result.Errors);
        return Result<Person>.Success(result.Value);
    }

    public static Result<LoggedUserRecord> LoggedUser(ClaimsPrincipal user)
    {
        return Result<LoggedUserRecord>.Success(new LoggedUserRecord
        (
            id: user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)!.Value,
            userName: user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value,
            email: user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value,
            role: user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value
        ));
    }

    public static async Task<IResult> Login(
                    [FromBody] LoginRecord input,
                    [FromServices] IPersonService service)
    {
        var blogUser = await service.GetUserByNameAsync(input.userName);
        if (!blogUser.IsSuccess) return Results.BadRequest(blogUser.Errors);
        var result = await service.LoginAsync(input.userName, input.Password);
        if (!result.IsSuccess) return Results.BadRequest(result.Errors);
        var token = result.Value;
        return Results.Ok(new { token });
    }

    public static async Task<IResult> Register(
                  [FromBody] UserRecordDTO input,
                  [FromServices] IPersonService service)
    {
        var blogUser = await service.RegisterAsync(input);
        if (!blogUser.IsSuccess) return Results.BadRequest(blogUser.Errors);
        return Results.Ok(blogUser.Value);
    }
}