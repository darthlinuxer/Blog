namespace Infra;
public class PostRepository : GenericRepository<PostModel>, IPostRepository
{
    public PostRepository(BlogContext context) : base(context)
    {
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByAuthorIdAsync(
        string authorId,
        int page,
        int count,
        bool descending,
        bool asNoTracking,
        string[]? navigation,
        CancellationToken ct)
    {
        return GetAllAsync(where: $"AuthorId==\"{authorId}\"",
                           orderby: "Title",
                           page: page,
                           count: count,
                           descending: descending,
                           asNoTracking: asNoTracking,
                           includeNavigationNames: navigation,
                           ct: ct);

    }
}