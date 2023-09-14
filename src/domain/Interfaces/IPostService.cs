namespace Domain.Interfaces;

public interface IPostService
{
    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllAsync(
                                                           CancellationToken ct,
                                                           string orderby = "PostId",
                                                           int page = 1,
                                                           int count = 10,
                                                           bool descending = true,
                                                           string[]? includeNavigationNames = null,
                                                           bool asNoTracking = true
                                                           );

    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllFilteredAsync(
                                                           string where,
                                                           CancellationToken ct,
                                                           string orderby = "PostId",
                                                           int page = 1,
                                                           int count = 10,
                                                           bool descending = true,
                                                           string[]? includeNavigationNames = null,
                                                           bool asNoTracking = true
                                                           );

    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByAuthorAsync(
        string author,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null
        );

    Task<Result<PostModel>> GetAsync(Expression<Func<PostModel, bool>> p,
                                CancellationToken ct,
                                bool asNoTracking,
                                string[]? includeNavigationNames);

    Task<Result<PostModel>> AddAsync(
        PostModelDTO entity);

    Task<Result<PostModel>> RemoveAsync(int postId, CancellationToken ct);

    Task<Result<PostModel>> UpdateAsync(
        PostModelDTO entity,
        CancellationToken ct
        );
}