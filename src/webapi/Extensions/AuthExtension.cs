

using Microsoft.AspNetCore.Identity;

namespace WebApi.Extensions;
public static class AuthExtension
{
    public static IServiceCollection ConfigureJwtAndPolicies(this IServiceCollection serviceColletion)
    {
        serviceColletion.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = IdentityConstants.BearerScheme;
                    options.DefaultChallengeScheme = IdentityConstants.BearerScheme;
                })
                .AddBearerToken(IdentityConstants.BearerScheme, options =>
                {
                    options.BearerTokenExpiration = TimeSpan.FromHours(1);
                });

        serviceColletion.AddAuthorization(config =>
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

        return serviceColletion;
    }
}