using System.Runtime.CompilerServices;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

public static partial class EndpointExtensions
{
    public static void MapCredentialsEndpoints(this WebApplication app)
    {
        var credentials = app.MapGroup("/api/credentials/");

        credentials.MapGet("loggeduser", (
            ClaimsPrincipal user) =>
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            return Results.Ok(new
            {
                Username = username,
                Role = role
            });
        }).RequireAuthorization();

        credentials.MapPost("login", async (
                    [FromBody] UserRecord input,
                    [FromServices] UserManager<BlogUser> userManager,
                    [FromServices] SignInManager<BlogUser> signInManager,
                    CancellationToken ct) =>
        {
            var blogUser = await userManager.FindByNameAsync(input.username);
            if (blogUser is not null) return Results.Ok("User already exist");

            var result = await signInManager.CheckPasswordSignInAsync(blogUser, input.password, false);
            if (!result.Succeeded) return Results.BadRequest("Username or password are invalid!");
            return Results.Ok(blogUser);
        });

        credentials.MapPost("register", async (
                  [FromBody] UserRecord input,
                  [FromServices] UserManager<BlogUser> userManager,
                  [FromServices] SignInManager<BlogUser> signInManager,
                  CancellationToken ct) =>
        {
            var blogUser = await userManager.FindByNameAsync(input.username);
            if (blogUser is not null) return Results.Ok("User already exist");

            blogUser = new BlogUser
            {
                UserName = input.username,
                PasswordHash = "My Internal Secret Password Hash",
                Email = input.email
            };

            var result = await userManager.CreateAsync(blogUser, input.password);
            if (!result.Succeeded) return Results.BadRequest(result.Errors);

            await userManager.AddClaimsAsync(blogUser, new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Writer")
            });

            return Results.Ok(new { msg = "User Created!", user = blogUser });
        });
    }
}