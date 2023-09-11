namespace WebApi.Extensions;
public static class DbContextExtensions
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
        return services;
    }
}