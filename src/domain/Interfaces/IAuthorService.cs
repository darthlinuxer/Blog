namespace Domain.Interfaces;

public interface IAuthorService
{
     Task<Result<IEnumerable<PostModel>>> GetAllPostsByUserAsync(string username);
}