namespace Application.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PostModel>> AddAsync(PostModel entity)
    {
        var addedPost = await _unitOfWork.Posts.AddAsync(entity);
        if (addedPost is null) return Result<PostModel>.Failure(["Post not added"]);
        return Result<PostModel>.Success(addedPost);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel>? GetAllAsync(string where,
                                                                  string orderby,
                                                                  int page,
                                                                  int count,
                                                                  bool descending,
                                                                  string[]? includeNavigationNames,
                                                                  bool asNoTracking,
                                                                  CancellationToken ct)
    {
        return _unitOfWork.Posts.GetAllAsync(where,
                                             orderby,
                                             page,
                                             count,
                                             descending,
                                             includeNavigationNames,
                                             asNoTracking,
                                             ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel>? GetAllByAuthorAsync(string username,
                                                                          int page,
                                                                          int count,
                                                                          bool descending,
                                                                          bool asNoTracking,
                                                                          string[]? navigation,
                                                                          CancellationToken ct)
    {
        return _unitOfWork.Posts.GetAllByAuthorAsync(username,
                                                     page,
                                                     count,
                                                     descending,
                                                     asNoTracking,
                                                     navigation,
                                                     ct);
    }

    public async Task<Result<PostModel>> GetAsync(Expression<Func<PostModel, bool>> p,
                                      CancellationToken ct,
                                      bool asNoTracking,
                                      string[]? includeNavigationNames)
    {
        var post = await _unitOfWork.Posts.GetAsync(p, ct, asNoTracking, includeNavigationNames);
        if (post is null) return Result<PostModel>.Failure(["Post does not exist"]);
        return Result<PostModel>.Success(post);
    }

    public Result<PostModel> Remove(PostModel entity)
    {
        var removedPost = _unitOfWork.Posts.Remove(entity);
        if (removedPost is null) return Result<PostModel>.Failure(["Post does not exist"]);
        return Result<PostModel>.Success(removedPost);
    }

    public Result<PostModel> Update(PostModel entity)
    {
        var updatedPost = _unitOfWork.Posts.Update(entity);
        if (updatedPost is null) return Result<PostModel>.Failure(["Post does not exist"]);
        return Result<PostModel>.Success(updatedPost);
    }

    public async Task<int> CompleteAsync() => await _unitOfWork.CompleteAsync();
}