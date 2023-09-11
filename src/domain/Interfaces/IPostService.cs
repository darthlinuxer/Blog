namespace Domain.Interfaces;

public interface IPostService
{
    ConfiguredCancelableAsyncEnumerable<PostModel>? GetAllAsync(string where,
                                                           string orderby,
                                                           int page,
                                                           int count,
                                                           bool descending,
                                                           string[]? includeNavigationNames,
                                                           bool asNoTracking,
                                                           CancellationToken ct);

    ConfiguredCancelableAsyncEnumerable<PostModel>? GetAllByAuthorAsync(string username,
                                                                   int page,
                                                                   int count,
                                                                   bool descending,
                                                                   bool asNoTracking,
                                                                   string[]? navigation,
                                                                   CancellationToken ct);

    Task<Result<PostModel>> GetAsync(Expression<Func<PostModel, bool>> p,
                                CancellationToken ct,
                                bool asNoTracking,
                                string[]? includeNavigationNames);

    Task<Result<PostModel>> AddAsync(PostModel entity);
    Result<PostModel> Remove(PostModel entity);
    Result<PostModel> Update(PostModel entity);
    Task<int> CompleteAsync();
}