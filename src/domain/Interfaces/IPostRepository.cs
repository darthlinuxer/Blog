namespace Domain.Interfaces;

public interface IPostRepository: IGenericRepository<PostModel>
{
    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorNameAsync(
        string author,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true
        );

    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorIdAsync(
       string authorId,
       CancellationToken ct,
       int page = 1,
       int count = 10,
       bool descending = true,
       bool asNoTracking = true,
       string[]? navigation = null
       );

    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByTitleAsync(
        string title,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null
        );

    ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByContentsAsync(
        string content,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null
        );

}