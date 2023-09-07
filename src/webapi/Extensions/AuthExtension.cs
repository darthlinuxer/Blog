

namespace WebApi.Extensions;
public static class AuthExtension
{
    public static IServiceCollection ConfigureJwtAndPolicies(this IServiceCollection serviceColletion)
    {
        serviceColletion.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = "BlogAPI",
                        ValidateIssuer = true,
                        RequireSignedTokens = true,
                        ValidAudience = "BlogUsers",
                        ValidateAudience = false,
                        IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(DefaultProperties.JWTTokenPwd) ?? "")),
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            context.Response.WriteAsJsonAsync(new
                            {
                                error = context.Error,
                                description = context.ErrorDescription
                            });
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            context.HttpContext.User = context.Principal!;
                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
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