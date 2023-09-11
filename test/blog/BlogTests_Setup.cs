namespace blog;

[TestClass]
public partial class BlogTest
{
    private ServiceProvider _serviceProvider;

    public BlogTest()
    {
        var services = new ServiceCollection();

        services
           .AddPooledDbContextFactory<BlogContext>(p => p.UseInMemoryDatabase("test.db"))
           .AddScoped<IUnitOfWork, UnitOfWork>()
           .AddScoped<IPostService, PostService>()
           .AddScoped<ICommentService, CommentService>()
           .AddScoped<IUserService, UserService>()
           .AddScoped(implementationFactory: sp => sp.GetRequiredService<IDbContextFactory<BlogContext>>()
                                                     .CreateDbContext());
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
               .AddBearerToken(IdentityConstants.BearerScheme, options =>
               {
                   options.BearerTokenExpiration = TimeSpan.FromHours(1);
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

        _serviceProvider = services.BuildServiceProvider();
    }

    [TestInitialize]
    public void InitRoles()
    {
        var roleManager = _serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = ["Writer", "Editor", "Public"];

        foreach (var role in roles)
        {
            if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
            }
        }
    }
}