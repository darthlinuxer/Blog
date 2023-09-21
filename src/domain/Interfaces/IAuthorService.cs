namespace Domain.Interfaces;

public interface IAuthorService
{
     Task<Result<IEnumerable<PostModel>>> GetAllPostsByAuthorAsync(string username);
}