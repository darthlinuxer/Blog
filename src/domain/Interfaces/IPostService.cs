namespace Domain.Interfaces;

public interface IPostService : IService<PostModel>
{

    Task<Result<IList<PostModel>>> RemoveAllByAuthorName(string name);

    Task<Result<PostModel>> GetPostByIdAsync(int id);
    Task<Result<PostModel>> GetPostByAuthorEmailAsync(string email);
    Task<Result<PostModel>> GetPostByAuthorNameAsync(string name);
    Task<Result<PostModel>> GetPostByAuthorIdAsync(string id);

    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllAsync(
                      int page,
                      int count,
                      Expression<Func<PostModel, string>> orderby,
                      bool descending,
                      bool noTracking,
                      bool includePosts,
                      bool includeComments,
                      CancellationToken ct);

    ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByAuthorNameAsync(
                     string author,
                     int page,
                     int count,
                     Expression<Func<PostModel, string>> orderby,
                     bool descending,
                     bool noTracking,
                     bool includePosts,
                     bool includeComments,
                     CancellationToken ct);

}