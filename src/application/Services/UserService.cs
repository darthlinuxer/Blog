namespace Application.Services;

public class UserService : IUserService
{
    private IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    Task<User?> IGenericRepository<User>.AddAsync(User entity)
    {
        return _unitOfWork.Users.AddAsync(entity);
    }

    public ConfiguredCancelableAsyncEnumerable<User>? GetAllAsync(string where,
                                                                  string orderby,
                                                                  int page,
                                                                  int count,
                                                                  bool descending,
                                                                  string[]? includeNavigationNames,
                                                                  bool asNoTracking,
                                                                  CancellationToken ct)
    {
        return _unitOfWork.Users.GetAllAsync(where,
                                             orderby,
                                             page,
                                             count,
                                             descending,
                                             includeNavigationNames,
                                             asNoTracking,
                                             ct);
    }

    public ConfiguredCancelableAsyncEnumerable<User>? GetAllUsersByRole(UserRole role,
                                                                        int page,
                                                                        int count,
                                                                        bool descending,
                                                                        bool asNoTracking,
                                                                        string[]? navigation,
                                                                        CancellationToken ct)
    {
        return _unitOfWork.Users.GetAllUsersByRole(role,
                                                   page,
                                                   count,
                                                   descending,
                                                   asNoTracking,
                                                   navigation,
                                                   ct);
    }

    public Task<User?> GetAsync(Expression<Func<User, bool>> p,
                                CancellationToken ct,
                                bool asNoTracking,
                                string[]? includeNavigationNames)
    {
        return _unitOfWork.Users.GetAsync(p, ct, asNoTracking, includeNavigationNames);
    }

    public User? Remove(User entity)
    {
        return _unitOfWork.Users.Remove(entity);
    }

    public User? Update(User entity)
    {
        return _unitOfWork.Users.Update(entity);
    }

    public async Task<int> CompleteAsync() => await _unitOfWork.CompleteAsync();
}