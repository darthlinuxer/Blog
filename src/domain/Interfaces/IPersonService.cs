namespace Domain.Interfaces;

public interface IPersonService
{
    Task<Result<Person>> RegisterAsync(UserRecordDTO input);

    Task<Result<Person>> GetUserByIdAsync(string id);

    Task<Result<Person>> GetUserByEmailAsync(string email);

    Task<Result<Person>> GetUserByNameAsync(string name);


    IAsyncEnumerable<object> GetAllUsersByRoleAsync(
                     string role,
                     int page,
                     int count,
                     string orderby,
                     bool descending,
                     bool noTracking,
                     CancellationToken ct);

    ConfiguredCancelableAsyncEnumerable<Person> GetAllCommentsByUserAsync(
                    string username,
                    int page,
                    int count,
                    Expression<Func<Person, string>> orderby,
                    bool descending,
                    bool noTracking,
                    bool includePosts,
                    CancellationToken ct);


    Task<Result<string>> LoginAsync(string username, string password);

    Task<Result<Person>> DeleteAccountWithId(string id);

    Task<Result<bool>> ChangePasswordAsync(Person user, string oldPassword, string newPassword);

    Task<Result<string>> ForgotPasswordAsync(string email);

    Task<Result<bool>> ResetPasswordAsync(string email, string token, string newPassword);

    Task<Result<bool>> IsUserInRoleAsync(string username, string role);

    Task<Result<bool>> IsUserIdInRoleAsync(string userId, string role);
}