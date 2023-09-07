namespace Domain.Interfaces;

public interface IPostService: IGenericRepository<Post>, IPostRepository
{
    public Task<int> CompleteAsync();    
}