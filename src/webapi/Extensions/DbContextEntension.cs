using Infra;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;
public static partial class ServiceExtentions
{
    public static IServiceCollection ConfigCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        services.AddPooledDbContextFactory<BlogContext>(options =>
        {
            options.UseInMemoryDatabase("Blog.db");
        });

        services.AddScoped(implementationFactory: sp => sp
              .GetRequiredService<IDbContextFactory<BlogContext>>()
              .CreateDbContext());

        return services;
    }
}