namespace Domain.Interfaces;

public interface ICommentService
{
    ConfiguredCancelableAsyncEnumerable<Comment>? GetAllCommentsForPostAsync(
       int postId,
       int page,
       int count,
       bool descending,
       bool asNoTracking,
       string[]? navigation,
       CancellationToken ct);

    ConfiguredCancelableAsyncEnumerable<Comment>? GetAllCommentsOfUserAsync(
        string username,
        int page,
        int count,
        bool descending,
        bool asNoTracking,
        string[]? navigation,
        CancellationToken ct);

    Task<Result<Comment>> GetAsync(Expression<Func<Comment, bool>> p,
                                   CancellationToken ct,
                                   bool asNoTracking,
                                   string[]? includeNavigationNames);

    Task<Result<Comment>> AddAsync(Comment entity);
    Result<Comment> Remove(Comment entity);
    Result<Comment> Update(Comment entity);

    public Task<int> CompleteAsync();
}