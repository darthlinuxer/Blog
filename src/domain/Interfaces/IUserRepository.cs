namespace Domain.Interfaces;

public interface IUserRepository: IGenericRepository<User>
{
    public ConfiguredCancelableAsyncEnumerable<User>? GetAllUsersByRole(UserRole role,
                                                                       int page,
                                                                       int count,
                                                                       bool descending,
                                                                       bool asNoTracking,
                                                                       string[]? navigation,
                                                                       CancellationToken ct);
    
}