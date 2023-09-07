namespace Domain.Interfaces;

public interface IUserService : IGenericRepository<User>, IUserRepository
{
    public Task<int> CompleteAsync();
}