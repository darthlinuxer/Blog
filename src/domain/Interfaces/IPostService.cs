namespace Domain.Interfaces;

public interface IPostService
{
  Task<Result<PostModel>> AddAsync(PostModelDTO entity);

  Task<Result<PostModel>> ChangePostStatus(ClaimsPrincipal principal, int postId, Status status, CancellationToken ct);

  ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllAsync(
      CancellationToken ct,
      string orderby = "PostId",
      int page = 1,
      int count = 10,
      bool descending = true,
      string[]? includeNavigationNames = null,
      bool asNoTracking = true,
      Status postStatus = Status.published);

  ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllFilteredAsync(
  string where,
  CancellationToken ct,
  string orderby = "PostId",
  int page = 1,
  int count = 10,
  bool descending = true,
  string[]? includeNavigationNames = null,
  bool asNoTracking = true);

  ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorNameAsync(
  string authorname,
  CancellationToken ct,
  string orderBy = "Title",
  int page = 1,
  int count = 10,
  bool descending = true,
  bool asNoTracking = true,
  Status postStatus = Status.published);

  ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorIdAsync(
    string authorId,
    CancellationToken ct,
    string orderBy = "Title",
    int page = 1,
    int count = 10,
    bool descending = true,
    bool asNoTracking = true,
    string[]? navigation = null,
    Status postStatus = Status.published);

  ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByTitleAsync(
    string title,
    CancellationToken ct,
    string orderBy = "Title",
    int page = 1,
    int count = 10,
    bool descending = true,
    bool asNoTracking = true,
    string[]? navigation = null,
    Status postStatus = Status.published);


  ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByContentsAsync(
  string content,
  CancellationToken ct,
  string orderBy = "Title",
  int page = 1,
  int count = 10,
  bool descending = true,
  bool asNoTracking = true,
  string[]? navigation = null,
  Status postStatus = Status.published);


  Task<Result<PostModel>> GetAsync(Expression<Func<PostModel, bool>> p,
                                   CancellationToken ct,
                                   bool asNoTracking = true,
                                   string[]? includeNavigationNames = null);

  Task<Result<PostModel>> UpdateAsync(PostModelDTO entity,
                                      CancellationToken ct);

  Task<Result<PostModel>> RemoveAsync(int postId, CancellationToken ct);

  bool Exist(Expression<Func<PostModel, bool>> p);

  int Count(Expression<Func<PostModel, bool>> p);
}