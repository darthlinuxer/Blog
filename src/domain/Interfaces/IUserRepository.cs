namespace Domain.Interfaces;

public interface IUserRepository : IGenericRepository<BlogUser>
{
    public ConfiguredCancelableAsyncEnumerable<BlogUser>? GetAllUsersByRole(UserRole role,
                                                                       int page,
                                                                       int count,
                                                                       bool descending,
                                                                       bool asNoTracking,
                                                                       string[]? navigation,
                                                                       CancellationToken ct);

}