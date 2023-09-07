namespace Domain.Interfaces;

public interface ICommentRepository : IGenericRepository<Comment>
{
    public ConfiguredCancelableAsyncEnumerable<Comment>? GetAllCommentsForPostAsync(
        int postId,
        int page,
        int count,
        bool descending,
        bool asNoTracking,
        string[]? navigation,
        CancellationToken ct);

    public ConfiguredCancelableAsyncEnumerable<Comment>? GetAllCommentsOfUserAsync(
        string username,
        int page,
        int count,
        bool descending,
        bool asNoTracking,
        string[]? navigation,
        CancellationToken ct);
}