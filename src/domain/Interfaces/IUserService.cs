namespace Domain.Interfaces;

public interface IUserService
{
    Task<Result<BaseUser>> RegisterAsync(UserRecordDTO input);

    Task<Result<BaseUser>> GetUserByIdAsync(string id);

    Task<Result<BaseUser>> GetUserByEmailAsync(string email);

    Task<Result<BaseUser>> GetUserByNameAsync(string name);

    ConfiguredCancelableAsyncEnumerable<BaseUser> GetAllUsersFiltered(
                      Expression<Func<BaseUser, bool>> where,
                      int page,
                      int count,
                      Expression<Func<BaseUser, string>> orderby,
                      bool descending,
                      bool noTracking,
                      CancellationToken ct);

    ConfiguredCancelableAsyncEnumerable<BaseUser> GetAll(
                      int page,
                      int count,
                      Expression<Func<BaseUser, string>> orderby,
                      bool descending,
                      bool noTracking,
                      bool includePosts,
                      CancellationToken ct);

    IAsyncEnumerable<BaseUser> GetAllUsersByRole(
                     string role,
                     int page,
                     int count,
                     Expression<Func<BaseUser, string>> orderby,
                     bool descending,
                     bool noTracking,
                     bool includePosts,
                     CancellationToken ct);


    Task<Result<string>> LoginAsync(string username, string password);

    Task<Result<BaseUser>> DeleteAccountWithId(string id);

    Task<Result<bool>> ChangePasswordAsync(BaseUser user, string oldPassword, string newPassword);

    Task<Result<string>> ForgotPasswordAsync(string email);

    Task<Result<bool>> ResetPasswordAsync(string email, string token, string newPassword);

    Task<Result<bool>> IsUserInRoleAsync(string username, string role);

    Task<Result<bool>> IsUserIdInRoleAsync(string userId, string role);

    Task<Result<IEnumerable<PostModel>>> GetAllPostsByUserAsync(string username);
}