using webapi.Extensions;

namespace WebApi.Extensions;
public static class AuthExtension
{
    public static IServiceCollection ConfigureJwtAndPolicies(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                    options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
                })
                .AddJwtBearer(IdentityConstants.BearerScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = TokenExtensions.Issuer,
                        ValidAudience = TokenExtensions.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenExtensions.SecretKey)),
                        ClockSkew = TimeSpan.FromSeconds(0)
                    };
                });

        services.AddAuthorization(config =>
        {
            config.AddPolicy("PublicPolicy", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.RequireRole("Public");
            });
            config.AddPolicy("WriterPolicy", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.RequireRole("Writer");
            });
            config.AddPolicy("EditorPolicy", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.RequireRole("Editor");
            });
            config.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        });

        return services;
    }
}