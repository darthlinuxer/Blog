using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace blog;

public class SharedSetup
{
    public ServiceProvider ServiceProvider;
    public IPostService PostService;
    public IUserService UserService;
    public ICommentService CommentService;
    public RoleManager<IdentityRole> RoleManager;

    private static int dbCounter = 0;

    public SharedSetup()
    {
        dbCounter++;
        var services = new ServiceCollection();
        // Add your services here...
        //The InMemory Database used to leak data between tests, therefore a new one is created
        //every time this constructor is called;
        services
          .AddPooledDbContextFactory<BlogContext>(p => p.UseInMemoryDatabase($"Db{dbCounter}.db"))
          .AddScoped<IUnitOfWork, UnitOfWork>()
          .AddScoped<IPostService, PostService>()
          .AddScoped<ICommentService, CommentService>()
          .AddScoped<IUserService, UserService>()
          .AddScoped(implementationFactory: sp => sp.GetRequiredService<IDbContextFactory<BlogContext>>()
                                                    .CreateDbContext())
          .AddTransient<IValidator<PostModelDTO>, PostModelDTOValidations>()
          .AddTransient<IValidator<PostLifeCycle>, PostLifeCycleValidations>();
        services.AddIdentityCore<BlogUser>(
          options =>
          {
              options.SignIn.RequireConfirmedAccount = false;
              options.SignIn.RequireConfirmedEmail = false;
              options.User.RequireUniqueEmail = true;
              options.Lockout.MaxFailedAccessAttempts = 5;
              options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
              options.Password.RequireDigit = true;
              options.Password.RequiredLength = 5;
              options.Password.RequireLowercase = true;
              options.Password.RequireUppercase = true;
          })
              .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<BlogContext>()
              .AddUserManager<UserManager<BlogUser>>()
              .AddSignInManager<SignInManager<BlogUser>>()
              .AddRoleManager<RoleManager<IdentityRole>>();

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

        services.Configure<JsonOptions>(options =>
        {
            options.JsonSerializerOptions.MaxDepth = 64;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        });

        ServiceProvider = services.BuildServiceProvider();
        RoleManager = ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        UserService = ServiceProvider.GetRequiredService<IUserService>();
        PostService = ServiceProvider.GetRequiredService<IPostService>();
        CommentService = ServiceProvider.GetRequiredService<ICommentService>();
    }

    public void SeedData()
    {
        string[] roles = ["Writer", "Editor", "Public"];

        foreach (var role in roles)
        {
            if (!RoleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
            {
                RoleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
            }
        }

        UserService.RegisterAsync(
           new UserRecordDTO(
               username: "admin",
               password: "ChangeMe1$",
               email: "editor@blog.com",
               role: "Editor"
           )
       ).GetAwaiter().GetResult();

        UserService.RegisterAsync(
               new UserRecordDTO(
                username: "darthlinuxer",
                password: "ChangeMe1$",
                email: "darthlinuxer@blog.com",
                role: "Writer"
            )
        ).GetAwaiter().GetResult();

        UserService.RegisterAsync(
              new UserRecordDTO(
               username: "luke",
               password: "ChangeMe1$",
               email: "luke@rebellalliance.com",
               role: "Public"
           )
       ).GetAwaiter().GetResult();
    }
}