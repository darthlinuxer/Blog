namespace Infra;
public class CommentRepository : GenericRepository<Comment>, ICommentRepository
{
  public CommentRepository(BlogContext context) : base(context)
  {
  }

  public ConfiguredCancelableAsyncEnumerable<Comment?> GetAllCommentsForPostAsync(
      int postId,
      int page,
      int count,
      bool descending,
      bool asNoTracking,
      string[]? navigation,
      CancellationToken ct)
  {
    return GetAllAsync(where: $"PostId={postId}",
                       orderby: "datetime",
                       page: page,
                       count: count,
                       descending: descending,
                       asNoTracking: asNoTracking,
                       includeNavigationNames: navigation,
                       ct: ct);

  }

  public ConfiguredCancelableAsyncEnumerable<Comment?> GetAllCommentsOfUserAsync(
      string username,
      int page,
      int count,
      bool descending,
      bool asNoTracking,
      string[]? navigation,
      CancellationToken ct)
  {
    return GetAllAsync(where: $"@Post.Author.UserName=\"{username}\"",
                       orderby: "datetime",
                       page: page,
                       count: count,
                       descending: descending,
                       asNoTracking: asNoTracking,
                       includeNavigationNames: ["Author"],
                       ct: ct);

  }
}