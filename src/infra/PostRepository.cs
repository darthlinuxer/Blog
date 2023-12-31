using Domain.Enums;

namespace Infra;
public class PostRepository : GenericRepository<PostModel>, IPostRepository
{
    public PostRepository(BlogContext context) : base(context)
    {
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllAsync(
       CancellationToken ct,
       string orderBy = "Title",
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null,
       PostStatus postStatus = PostStatus.published)
    {
        return GetAllAsync(where: $"PostId > 0 && PostStatus == \"{postStatus}\"",
                           orderby: orderBy,
                           page: page,
                           count: count,
                           descending: descending,
                           includeNavigationNames: navigation,
                           asNoTracking: asNoTracking,
                           ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllFilteredAsync(
       string where,
       CancellationToken ct,
       string orderBy = "Title",
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null)
    {
        return GetAllAsync(where: where,
                           orderby: orderBy,
                           page: page,
                           count: count,
                           descending: descending,
                           includeNavigationNames: navigation,
                           asNoTracking: asNoTracking,
                           ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorIdAsync(string authorId,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        PostStatus postStatus = PostStatus.published)
    {
        return GetAllAsync(
            where: $"AuthorId == \"{authorId}\" && PostStatus == \"{postStatus}\"",
            orderby: orderBy,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: navigation,
            asNoTracking: asNoTracking,
            ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorNameAsync(
        string author,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        PostStatus postStatus = PostStatus.published)
    {
        return GetAllAsync(
            where: $"@Author.Username == \"{author}\" && PostStatus == \"{postStatus}\"",
            orderby: orderBy,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: ["Author", "Comments"],
            asNoTracking: asNoTracking,
            ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByContentsAsync(
        string content,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        PostStatus postStatus = PostStatus.published)
    {
        return GetAllAsync(
            where: $"@Content.Contains(\"{content}\") && PostStatus == \"{postStatus}\"",
            orderby: orderBy,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: navigation,
            asNoTracking: asNoTracking,
            ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByTitleAsync(
        string title,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        PostStatus postStatus = PostStatus.published)
    {
        return GetAllAsync(
            where: $"@Title.Contains(\"{title}\") && PostStatus == \"{postStatus}\"",
            orderby: orderBy,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: navigation,
            asNoTracking: asNoTracking,
            ct: ct);
    }

}