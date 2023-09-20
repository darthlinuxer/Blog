namespace Domain.Interfaces;

public interface IArticleRepository : IGenericRepository<Article>
{
    ConfiguredCancelableAsyncEnumerable<Article?> GetAllAsync(
       CancellationToken ct,
       string orderBy = "Title",
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null,
       Status postStatus = Status.published
       );

    ConfiguredCancelableAsyncEnumerable<Article?> GetAllFilteredAsync(
      string where,
      CancellationToken ct,
      string orderBy = "Title",
      int page = 1,
      int count = 10,
      bool descending = true,
      bool asNoTracking = true,
      string[]? navigation = null
      );

    ConfiguredCancelableAsyncEnumerable<Article?> GetAllByAuthorNameAsync(
        string author,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        Status status = Status.published
        );

    ConfiguredCancelableAsyncEnumerable<Article?> GetAllByAuthorIdAsync(
       string authorId,
       CancellationToken ct,
       string orderBy = "Title",
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null,
       Status postStatus = Status.published
       );

    ConfiguredCancelableAsyncEnumerable<Article?> GetAllByTitleAsync(
        string title,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        Status postStatus = Status.published
        );

    ConfiguredCancelableAsyncEnumerable<Article?> GetAllByContentsAsync(
        string content,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        Status postStatus = Status.published
        );

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllCommentsForPostAsync(
      int postId,
      int page,
      int count,
      bool descending,
      bool asNoTracking,
      string[]? navigation,
      CancellationToken ct);

    public ConfiguredCancelableAsyncEnumerable<Article?> GetAllCommentsOfUserAsync(
        string username,
        int page,
        int count,
        bool descending,
        bool asNoTracking,
        string[]? navigation,
        CancellationToken ct);

}