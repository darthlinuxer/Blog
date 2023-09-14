namespace Infra;
public class PostRepository : GenericRepository<PostModel>, IPostRepository
{
    public PostRepository(BlogContext context) : base(context)
    {
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByAuthorAsync(
        string author,
        int page,
        int count,
        bool descending,
        bool asNoTracking,
        string[]? navigation,
        CancellationToken ct)
    {
        return GetAllAsync(where: $"@Author.UserName==\"{author}\"",
                           orderby: "Title",
                           page: page,
                           count: count,
                           descending: descending,
                           asNoTracking: asNoTracking,
                           includeNavigationNames: navigation,
                           ct: ct);

    }
}