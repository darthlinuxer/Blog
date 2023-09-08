using Microsoft.AspNetCore.Identity;

namespace blog;

[TestClass]
public partial class BlogTest
{
    ServiceProvider _serviceProvider;

    public BlogTest()
    {
        var services = new ServiceCollection();

        services
           .AddPooledDbContextFactory<BlogContext>(p => p.UseInMemoryDatabase("test.db"))
           .AddScoped<IUnitOfWork, UnitOfWork>()
           .AddScoped<IPostService, PostService>()
           .AddScoped<ICommentService, CommentService>()
           .AddSingleton(new JwtTokenService("secret_key"))
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
              .AddEntityFrameworkStores<BlogContext>()
              .AddUserManager<UserManager<BlogUser>>();

        _serviceProvider = services.BuildServiceProvider();
    }

}