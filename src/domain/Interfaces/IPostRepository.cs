namespace Domain.Interfaces;
public interface IPostRepository : IGenericRepository<Post>
{
    public Task<Post> GetByIdAsync(int id);
    public Task<IEnumerable<Post>> GetAllByAuthorAsync(User user);
}