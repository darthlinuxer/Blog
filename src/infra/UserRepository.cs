namespace Infra;
public class UserRepository : GenericRepository<User>, IUserRepository
{
  public UserRepository(BlogContext context) : base(context)
  {
  }

    public ConfiguredCancelableAsyncEnumerable<User>? GetAllUsersByRole(UserRole role,
                                                                       int page,
                                                                       int count,
                                                                       bool descending,
                                                                       bool asNoTracking,
                                                                       string[]? navigation,
                                                                       CancellationToken ct)
    {
        return GetAllAsync(where: $"Role={role}",
                           orderby: "Username",
                           page: page,
                           count: count,
                           descending: descending,
                           includeNavigationNames: navigation,
                           asNoTracking: asNoTracking,
                           ct: ct);
    }
}