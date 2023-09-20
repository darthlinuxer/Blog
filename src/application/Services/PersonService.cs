namespace Application.Services;

public class PersonService: IPersonService<Person>
{
    protected readonly UserManager<Person> _userManager;
    protected readonly SignInManager<Person> _signInManager;
    protected readonly RoleManager<IdentityRole> _roleManager;

    public PersonService(
        UserManager<Person> userManager,
        SignInManager<Person> signInManager,
        RoleManager<IdentityRole> roleManager
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public async Task<Result<Person>> RegisterAsync(UserRecordDTO input)
    {
         var roleInDb = await _roleManager.RoleExistsAsync(input.role);
        if (roleInDb is false)
            return Result<Person>.Failure([$"Role {input.role} is invalid!"]);

        var person = await _userManager.FindByNameAsync(input.username);
        if (person is not null) return Result<Person>.Failure(["User already exists"]);


        var userToAdd = new Person
        {
            UserName = input.username,
            PasswordHash = "My Internal Secret Password Hash",
            Email = input.email
        };

        var result = await _userManager.CreateAsync(userToAdd, input.password);
        if (!result.Succeeded)
            return Result<Person>.Failure(
                result.Errors.Select(e => e.Description).ToList<string>());

        var roleAddResult = await _userManager.AddToRoleAsync(userToAdd, input.role);
        if (!roleAddResult.Succeeded) return Result<Person>.Failure(roleAddResult.Errors.Select(c => c.Description).ToList());
        return Result<Person>.Success(userToAdd);

    }

    public async Task<Result<Person>> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is null) return Result<Person>.Failure(["Not found"]);
        return Result<Person>.Success(user);
    }

    public async Task<Result<Person>> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) return Result<Person>.Failure(["Not found"]);
        return Result<Person>.Success(user);
    }

    public async Task<Result<Person>> GetUserByNameAsync(string name)
    {
        var user = await _userManager.FindByNameAsync(name);
        if (user is null) return Result<Person>.Failure(["Not found"]);
        return Result<Person>.Success(user);
    }

    public ConfiguredCancelableAsyncEnumerable<Person> GetAllUsersFiltered(
                        Expression<Func<Person, bool>> where,
                        int page,
                        int count,
                        Expression<Func<Person, string>> orderby,
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

    public async IAsyncEnumerable<Person> GetAllUsersByRole(
                     string role,
                     int page,
                     int count,
                     Expression<Func<Person, string>> orderby,
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


    public ConfiguredCancelableAsyncEnumerable<Person> GetAll(
                      int page,
                      int count,
                      Expression<Func<Person, string>> orderby,
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
        var token = TokenExtensions.CreateToken(user as Person, roles);
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

    public async Task<Result<bool>> ChangePasswordAsync(Person user, string oldPassword, string newPassword)
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
    public async Task<Result<Person>> DeleteAccountWithId(string id)
    {
        var userExistResult = await GetUserByIdAsync(id);
        if (!userExistResult.IsSuccess) return Result<Person>.Failure(userExistResult.Errors);
        var result = await _userManager.DeleteAsync(userExistResult.Value);
        if (!result.Succeeded) return Result<Person>.Failure(result.Errors.Select(c => c.Description).ToList());
        return Result<Person>.Success(userExistResult.Value);
    }

    //---------------------------------------------------------------------------
}