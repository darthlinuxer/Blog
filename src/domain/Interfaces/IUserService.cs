namespace Domain.Interfaces;

public interface IUserService
{
    Task<Result<BlogUser>> RegisterAsync(UserRecordDTO input);

    Task<Result<BlogUser>> GetUserByIdAsync(string id);

    Task<Result<BlogUser>> GetUserByEmailAsync(string email);

    Task<Result<BlogUser>> GetUserByNameAsync(string name);

    Task<Result<IEnumerable<BlogUser>>> GetAllUsersByRoleAsync(string role);

    Task<Result<string>> LoginAsync(string username, string password);

    Task<Result<BlogUser>> DeleteAccountWithId(string id);

    Task<Result<bool>> ChangePasswordAsync(BlogUser user, string oldPassword, string newPassword);

    Task<Result<string>> ForgotPasswordAsync(string email);

    Task<Result<bool>> ResetPasswordAsync(string email, string token, string newPassword);

    Task<Result<bool>> IsUserInRoleAsync(string username, string role);

    Task<Result<bool>> IsUserIdInRoleAsync(string userId, string role);

    Task<Result<IEnumerable<PostModel>>> GetAllPostsByUserAsync(string username);
}