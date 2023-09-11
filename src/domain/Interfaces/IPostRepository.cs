namespace Domain.Interfaces;

public interface IPostRepository : IGenericRepository<PostModel>
{
    public ConfiguredCancelableAsyncEnumerable<PostModel>? GetAllByAuthorAsync(string username,
                                                                          int page,
                                                                          int count,
                                                                          bool descending,
                                                                          bool asNoTracking,
                                                                          string[]? navigation,
                                                                          CancellationToken ct);
}