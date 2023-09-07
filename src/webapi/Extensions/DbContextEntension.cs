using Infra;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;
public static partial class ServiceExtentions
{
    public static IServiceCollection ConfigCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPooledDbContextFactory<BlogContext>(options =>
        {
            options.UseInMemoryDatabase("Blog.db");
        });

        services.AddScoped(implementationFactory: sp => sp
              .GetRequiredService<IDbContextFactory<BlogContext>>()
              .CreateDbContext());
        
        services.AddIdentityCore<User>()
                .AddEntityFrameworkStores<BlogContext>()
                .AddApiEndpoints();

        return services;
    }
}