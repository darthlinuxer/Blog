namespace Domain.Interfaces;

public interface IUserService : IGenericRepository<BlogUser>, IUserRepository
{
    public Task<int> CompleteAsync();
}