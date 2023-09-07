using System.Runtime.CompilerServices;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

public static partial class EndpointExtensions
{
    public static void MapCredentialsEndpoints(this WebApplication app)
    {
        var credentials = app.MapGroup("/api/credentials/");

        credentials.MapGet("", [Authorize] (
            ClaimsPrincipal user) =>
        {
            var username = user.FindFirst(ClaimTypes.Name)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            return new
            {
                Username = username,
                Role = role
            };
        });
    }
}