namespace webapi.Extensions;

public static class ScopedServices
{
    public static IServiceCollection AddCustomScopedServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}