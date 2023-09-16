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

    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByAuthorNameAsync(
        string author,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true
        );

    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByAuthorIdAsync(
       string authorId,
       CancellationToken ct,
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null
       );

    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByTitleAsync(
        string title,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null
        );

    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByContentsAsync(
        string content,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null
        );

    Task<Result<PostModel>> GetAsync(Expression<Func<PostModel, bool>> p,
                                CancellationToken ct,
                                bool asNoTracking = true,
                                string[]? includeNavigationNames = null);

    Task<Result<PostModel>> AddAsync(
        PostModelDTO entity);

    Task<Result<PostModel>> RemoveAsync(int postId, CancellationToken ct);

    Task<Result<PostModel>> UpdateAsync(
        PostModelDTO entity,
        CancellationToken ct
        );

}