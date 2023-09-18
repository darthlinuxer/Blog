using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<BaseUser> _userManager;
    private readonly SignInManager<BaseUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserService(
        UserManager<BaseUser> userManager,
        SignInManager<BaseUser> signInManager,
        RoleManager<IdentityRole> roleManager
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<Result<BaseUser>> RegisterAsync(UserRecordDTO input)
    {
        var roleInDb = await _roleManager.RoleExistsAsync(input.role);
        if (roleInDb is false)
            return Result<BaseUser>.Failure([$"Role {input.role} is invalid!"]);

        var blogUser = await _userManager.FindByNameAsync(input.username);
        if (blogUser is not null) return Result<BaseUser>.Failure(["User already exists"]);

        blogUser = new Author
        {
            UserName = input.username,
            PasswordHash = "My Internal Secret Password Hash",
            Email = input.email
        };

        var result = await _userManager.CreateAsync(blogUser, input.password);
        if (!result.Succeeded)
            return Result<BaseUser>.Failure(
                result.Errors.Select(e => e.Description).ToList<string>());

        var roleAddResult = await _userManager.AddToRoleAsync(blogUser, input.role);
        if (!roleAddResult.Succeeded) return Result<BaseUser>.Failure(roleAddResult.Errors.Select(c => c.Description).ToList());
        return Result<BaseUser>.Success(blogUser);
    }

    public async Task<Result<BaseUser>> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return Result<BaseUser>.Failure(["Not found"]);
        return Result<BaseUser>.Success(user);
    }

    public async Task<Result<BaseUser>> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return Result<BaseUser>.Failure(["Not found"]);
        return Result<BaseUser>.Success(user);
    }

    public async Task<Result<BaseUser>> GetUserByNameAsync(string name)
    {
        var user = await _userManager.FindByNameAsync(name);
        if (user is null) return Result<BaseUser>.Failure(["Not found"]);
        return Result<BaseUser>.Success(user);
    }

    public ConfiguredCancelableAsyncEnumerable<BaseUser> GetAllUsersFiltered(
                        Expression<Func<BaseUser, bool>> where,
                        int page,
                        int count,
                        Expression<Func<BaseUser, string>> orderby,
                        bool descending,
                        bool noTracking,
                        CancellationToken ct)
    {
        var users = _userManager.Users.AsQueryable();
        users = users.Where(where).Skip((page - 1) * count).Take(count);
        if (descending) users = users.OrderByDescending(orderby);
        else users = users.OrderBy(orderby);
        if (noTracking) users = users.AsNoTrackingWithIdentityResolution();
        return users.AsAsyncEnumerable().WithCancellation(ct);
    }

    public async IAsyncEnumerable<BaseUser> GetAllUsersByRole(
                     string role,
                     int page,
                     int count,
                     Expression<Func<BaseUser, string>> orderby,
                     bool descending,
                     bool noTracking,
                     bool includePosts,
                     [EnumeratorCancellation] CancellationToken ct)
    {
        var usersResult = GetAll(page, count, orderby, descending, noTracking, includePosts, ct);
        await foreach (var user in usersResult.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            if (await _userManager.IsInRoleAsync(user, role)) yield return user;
        }
    }


    public ConfiguredCancelableAsyncEnumerable<BaseUser> GetAll(
                      int page,
                      int count,
                      Expression<Func<BaseUser, string>> orderby,
                      bool descending,
                      bool noTracking,
                      bool includePosts,
                      CancellationToken ct)
    {
        var users = _userManager.Users.AsQueryable();
        users = users.Skip((page - 1) * count).Take(count);
        if (descending) users = users.OrderByDescending(orderby);
        else users = users.OrderBy(orderby);
        if (noTracking) users = users.AsNoTrackingWithIdentityResolution();
        if (includePosts) users = users.Include("Posts");
        return users.AsAsyncEnumerable().WithCancellation(ct);
    }

    //----------------------------------------------------------------------------------------
    public async Task<Result<string>> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null) return Result<string>.Failure(["User does not exist!"]);
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded) return Result<string>.Failure(["Invalid Password!"]);
        var roles = await _userManager.GetRolesAsync(user);
        var token = TokenExtensions.CreateToken(user, roles);
        return Result<string>.Success(token);
    }

    public async Task<Result<bool>> IsUserInRoleAsync(string username, string role)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user is null) return Result<bool>.Failure([$"User {username} does not exist!"]);
        var isInRole = await _userManager.IsInRoleAsync(user, role);
        if (!isInRole) return Result<bool>.Failure([$"User {username} is not in role {role}"]);
        return Result<bool>.Success(isInRole);
    }

    public async Task<Result<bool>> IsUserIdInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return Result<bool>.Failure([$"UserID {userId} does not exist!"]);
        var isInRole = await _userManager.IsInRoleAsync(user, role);
        if (!isInRole) return Result<bool>.Failure([$"UserID {userId} is not in role {role}"]);
        return Result<bool>.Success(isInRole);
    }

    public async Task<Result<bool>> ChangePasswordAsync(BaseUser user, string oldPassword, string newPassword)
    {
        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!result.Succeeded) return Result<bool>.Failure(result.Errors.Select(x => x.Description).ToList());
        return Result<bool>.Success(true);
    }

    public async Task<Result<string>> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return Result<string>.Failure(["User does not exist!"]);
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        //send this token to the user EMAIL. This service is not implemented yet!
        return Result<string>.Success(token);
    }

    public async Task<Result<bool>> ResetPasswordAsync(string email, string token, string newPassword)
    {
        ;
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return Result<bool>.Failure(["User does not exist!"]);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
        if (!result.Succeeded) return Result<bool>.Failure(result.Errors.Select(c => c.Description).ToList());
        return Result<bool>.Success(true);
    }

    //---------------------------------------------------------------------------
    public async Task<Result<BaseUser>> DeleteAccountWithId(string id)
    {
        var userExistResult = await GetUserByIdAsync(id);
        if (!userExistResult.IsSuccess) return Result<BaseUser>.Failure(userExistResult.Errors);
        var result = await _userManager.DeleteAsync(userExistResult.Value);
        if (!result.Succeeded) return Result<BaseUser>.Failure(result.Errors.Select(c => c.Description).ToList());
        return Result<BaseUser>.Success(userExistResult.Value);
    }

    //---------------------------------------------------------------------------

    public async Task<Result<IEnumerable<PostModel>>> GetAllPostsByUserAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return Result<IEnumerable<PostModel>>.Success(user!.Posts);
    }
}