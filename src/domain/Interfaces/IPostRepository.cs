namespace Domain.Interfaces;

public interface IPostRepository : IGenericRepository<PostModel>
{
    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllAsync(
       CancellationToken ct,
       string orderBy = "Title",
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null,
       PostStatus postStatus = PostStatus.published
       );

     ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllFilteredAsync(
       string where,
       CancellationToken ct,
       string orderBy = "Title",
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null
       );

    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorNameAsync(
        string author,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        PostStatus postStatus = PostStatus.published
        );

    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorIdAsync(
       string authorId,
       CancellationToken ct,
       string orderBy = "Title",
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null,
       PostStatus postStatus = PostStatus.published
       );

    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByTitleAsync(
        string title,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        PostStatus postStatus = PostStatus.published
        );

    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByContentsAsync(
        string content,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        PostStatus postStatus = PostStatus.published
        );

}