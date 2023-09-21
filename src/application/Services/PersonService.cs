namespace Application.Services;

public class PersonService: IPersonService
{
    protected readonly UserManager<Person> _userManager;
    protected readonly SignInManager<Person> _signInManager;
    protected readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;

    public PersonService(
        UserManager<Person> userManager,
        SignInManager<Person> signInManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        this._unitOfWork = unitOfWork;
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

    public ConfiguredCancelableAsyncEnumerable<Person> GetAllCommentsByUserAsync(
        string username,
        int page,
        int count,
        Expression<Func<Person, string>> orderby,
        bool descending,
        bool noTracking,
        bool includePosts,
        CancellationToken ct)
    {
        throw new NotImplementedException();
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


    //---------------------------------POLIMORPHIC------------------------------------------

    public async IAsyncEnumerable<object> GetAllUsersByRoleAsync(
        string role,
        int page,
        int count,
        string orderby,
        bool descending,
        bool noTracking,
        [EnumeratorCancellation] CancellationToken ct)
    { 
        if(string.Compare(role, "Writer")==0){
            var authors =  _unitOfWork.Persons.GetAllAsync<Author>(
                where: "Id>0",
                orderby: orderby,
                page: page,
                count:count,
                descending: descending,
                includeNavigationNames: ["Posts","Comments"],
                noTracking, ct);
            await foreach(var author in authors)
            {
                if(ct.IsCancellationRequested) yield break;
                yield return author;
            }
        }

         if(string.Compare(role, "Editor")==0){
            var editors =  _unitOfWork.Persons.GetAllAsync<Editor>(
                where: "Id>0",
                orderby: orderby,
                page: page,
                count:count,
                descending: descending,
                includeNavigationNames: ["Comments"],
                noTracking, ct);
            await foreach(var editor in editors)
            {
                if(ct.IsCancellationRequested) yield break;
                yield return editor;
            }
        }

        if(string.Compare(role, "Public")==0){
            var users =  _unitOfWork.Persons.GetAllAsync<Editor>(
                where: "Id>0",
                orderby: orderby,
                page: page,
                count:count,
                descending: descending,
                includeNavigationNames: ["Comments"],
                noTracking, ct);
            await foreach(var user in users)
            {
                if(ct.IsCancellationRequested) yield break;
                yield return user;
            }
        }
        
    }


}