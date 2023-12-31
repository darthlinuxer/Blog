namespace WebApi.Extensions;

public static class IdentityCoreExtensions
{
    public static IServiceCollection ConfigIdentityCore(this IServiceCollection services)
    {
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
                .AddSignInManager<SignInManager<BlogUser>>()
                .AddUserManager<UserManager<BlogUser>>()
                .AddRoleManager<RoleManager<IdentityRole>>();

        return services;
    }
}