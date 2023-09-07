namespace Application.Services;

public class UserService : IUserService
{
    private IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    Task<BlogUser?> IGenericRepository<BlogUser>.AddAsync(BlogUser entity)
    {
        return _unitOfWork.Users.AddAsync(entity);
    }

    public ConfiguredCancelableAsyncEnumerable<BlogUser>? GetAllAsync(string where,
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

    public ConfiguredCancelableAsyncEnumerable<BlogUser>? GetAllUsersByRole(UserRole role,
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

    public Task<BlogUser?> GetAsync(Expression<Func<BlogUser, bool>> p,
                                CancellationToken ct,
                                bool asNoTracking,
                                string[]? includeNavigationNames)
    {
        return _unitOfWork.Users.GetAsync(p, ct, asNoTracking, includeNavigationNames);
    }

    public BlogUser? Remove(BlogUser entity)
    {
        return _unitOfWork.Users.Remove(entity);
    }

    public BlogUser? Update(BlogUser entity)
    {
        return _unitOfWork.Users.Update(entity);
    }

    public async Task<int> CompleteAsync() => await _unitOfWork.CompleteAsync();
}