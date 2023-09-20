namespace Domain.Interfaces;

public interface IPersonService<T> where T : class
{
    Task<Result<T>> RegisterAsync(UserRecordDTO input);

    Task<Result<T>> GetUserByIdAsync(string id);

    Task<Result<T>> GetUserByEmailAsync(string email);

    Task<Result<T>> GetUserByNameAsync(string name);

    ConfiguredCancelableAsyncEnumerable<T> GetAllUsersFiltered(
                      Expression<Func<T, bool>> where,
                      int page,
                      int count,
                      Expression<Func<T, string>> orderby,
                      bool descending,
                      bool noTracking,
                      CancellationToken ct);

    ConfiguredCancelableAsyncEnumerable<T> GetAll(
                      int page,
                      int count,
                      Expression<Func<T, string>> orderby,
                      bool descending,
                      bool noTracking,
                      bool includePosts,
                      CancellationToken ct);

    IAsyncEnumerable<T> GetAllUsersByRole(
                     string role,
                     int page,
                     int count,
                     Expression<Func<T, string>> orderby,
                     bool descending,
                     bool noTracking,
                     bool includePosts,
                     CancellationToken ct);


    Task<Result<string>> LoginAsync(string username, string password);

    Task<Result<T>> DeleteAccountWithId(string id);

    Task<Result<bool>> ChangePasswordAsync(T user, string oldPassword, string newPassword);

    Task<Result<string>> ForgotPasswordAsync(string email);

    Task<Result<bool>> ResetPasswordAsync(string email, string token, string newPassword);

    Task<Result<bool>> IsUserInRoleAsync(string username, string role);

    Task<Result<bool>> IsUserIdInRoleAsync(string userId, string role);
}