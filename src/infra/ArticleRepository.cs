using Domain.Enums;

namespace Infra;
public class ArticleRepository : GenericRepository<Article>, IArticleRepository
{
    public ArticleRepository(BlogContext context) : base(context)
    {
    }

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllAsync(
       CancellationToken ct,
       string orderBy = "Title",
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null,
       Status postStatus = Status.published)
    {
        return GetAllAsync(where: $"Id > 0 && Status == \"{postStatus}\"",
                           orderby: orderBy,
                           page: page,
                           count: count,
                           descending: descending,
                           includeNavigationNames: navigation,
                           asNoTracking: asNoTracking,
                           ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllFilteredAsync(
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

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllByAuthorIdAsync(string authorId,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        Status postStatus = Status.published)
    {
        return GetAllAsync(
            where: $"AuthorId == \"{authorId}\" && Status == \"{postStatus}\"",
            orderby: orderBy,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: navigation,
            asNoTracking: asNoTracking,
            ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllByAuthorNameAsync(
        string author,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        Status status = Status.published)
    {
        return GetAllOfTypeAsync<Author>(
            where: $"@Author.UserName == \"{author}\" && Status == \"{status}\"",
            orderby: orderBy,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: ["Author", "Comments"],
            asNoTracking: asNoTracking,
            ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllByContentsAsync(
        string content,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        Status postStatus = Status.published)
    {
        return GetAllAsync(
            where: $"@Content.Contains(\"{content}\") && Status == \"{postStatus}\"",
            orderby: orderBy,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: navigation,
            asNoTracking: asNoTracking,
            ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllByTitleAsync(
        string title,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        Status postStatus = Status.published)
    {
        return GetAllAsync(
            where: $"@Title.Contains(\"{title}\") && Status == \"{postStatus}\"",
            orderby: orderBy,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: navigation,
            asNoTracking: asNoTracking,
            ct: ct);
    }

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllCommentsForPostAsync(
      int postId,
      int page,
      int count,
      bool descending,
      bool asNoTracking,
      string[]? navigation,
      CancellationToken ct)
    {
        return GetAllOfTypeAsync<Comment>(where: $"PostId={postId}",
                           orderby: "datetime",
                           page: page,
                           count: count,
                           descending: descending,
                           asNoTracking: asNoTracking,
                           includeNavigationNames: navigation,
                           ct: ct);

    }

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllCommentsOfUserAsync(
        string username,
        int page,
        int count,
        bool descending,
        bool asNoTracking,
        string[]? navigation,
        CancellationToken ct)
    {
        return GetAllOfTypeAsync<Comment>(where: $"@Post.Author.UserName=\"{username}\"",
                           orderby: "datetime",
                           page: page,
                           count: count,
                           descending: descending,
                           asNoTracking: asNoTracking,
                           includeNavigationNames: ["Author"],
                           ct: ct);

    }

}