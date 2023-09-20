
namespace Application.Services;

public class AuthorService : PersonService, IAuthorService
{
    public AuthorService(UserManager<Person> userManager,
                         SignInManager<Person> signInManager,
                         RoleManager<IdentityRole> roleManager) : base(userManager, signInManager, roleManager)
    {


    }

    public async Task<Result<IEnumerable<PostModel>>> GetAllPostsByUserAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null) return Result<IEnumerable<PostModel>>.Failure([$"{username} does not exit!"]);
        return Result<IEnumerable<PostModel>>.Success((user as Author).Posts);
    }

}