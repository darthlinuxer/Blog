namespace WebApi.Extensions;

public static class RoleInitializer
{
    public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["Writer", "Editor", "Public"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
