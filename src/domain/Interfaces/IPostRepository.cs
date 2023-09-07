namespace Domain.Interfaces;

public interface IPostRepository: IGenericRepository<Post>
{
    public ConfiguredCancelableAsyncEnumerable<Post>? GetAllByAuthorAsync(string username,
                                                                          int page,
                                                                          int count,
                                                                          bool descending,
                                                                          bool asNoTracking,
                                                                          string[]? navigation,
                                                                          CancellationToken ct);
}