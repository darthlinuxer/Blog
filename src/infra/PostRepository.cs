namespace Infra;
public class PostRepository : GenericRepository<PostModel>, IPostRepository
{
    public PostRepository(BlogContext context) : base(context)
    {
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorIdAsync(string authorId, CancellationToken ct, int page = 1, int count = 10, bool descending = true, bool asNoTracking = true, string[]? navigation = null)
    {
        return GetAllAsync($"AuthorId == \"{authorId}\"", orderby: "Title", page, count, descending, navigation, asNoTracking, ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorNameAsync(string author, CancellationToken ct, int page = 1, int count = 10, bool descending = true, bool asNoTracking = true)
    {
           return GetAllAsync($"@Author.Username == \"{author}\"", orderby: "Title", page, count, descending, includeNavigationNames:["Author","Comments"], asNoTracking, ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByContentsAsync(string content, CancellationToken ct, int page = 1, int count = 10, bool descending = true, bool asNoTracking = true, string[]? navigation = null)
    {
         return GetAllAsync($"@Content.Contains(\"{content}\")", orderby: "Title", page, count, descending, navigation, asNoTracking, ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByTitleAsync(string title, CancellationToken ct, int page = 1, int count = 10, bool descending = true, bool asNoTracking = true, string[]? navigation = null)
    {
         return GetAllAsync($"@Title.Contains(\"{title}\")", orderby: "Title", page, count, descending, navigation, asNoTracking, ct);
    }

}