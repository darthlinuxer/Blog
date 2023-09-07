using Microsoft.AspNetCore.Identity;

namespace blog;

[TestClass]
public partial class BlogTest
{
    ServiceProvider _serviceProvider;

    public BlogTest()
    {
        _serviceProvider = new ServiceCollection()
           .AddPooledDbContextFactory<BlogContext>(p => p.UseInMemoryDatabase("test.db"))
           .AddScoped<IUnitOfWork, UnitOfWork>()
           .AddScoped<IPostService, PostService>()
           .AddScoped<ICommentService, CommentService>()
           .AddSingleton(new JwtTokenService("secret_key"))
           .AddScoped(implementationFactory: sp => sp.GetRequiredService<IDbContextFactory<BlogContext>>()
                                                     .CreateDbContext())
           .AddScoped<UserManager<BlogUser>>()
           .BuildServiceProvider();
    }

}