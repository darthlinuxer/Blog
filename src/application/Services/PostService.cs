using System.Runtime.CompilerServices;

namespace Application.Services;

public class PostService: IPostService
{
    private readonly IUnitOfWork _unitOfWork;

    public PostService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Post?> AddAsync(Post entity)
    {
        return await _unitOfWork.Posts.AddAsync(entity);
    }

    public ConfiguredCancelableAsyncEnumerable<Post>? GetAllAsync(string where,
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

    public ConfiguredCancelableAsyncEnumerable<Post>? GetAllByAuthorAsync(string username,
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

    public async Task<Post?> GetAsync(Expression<Func<Post, bool>> p,
                                      CancellationToken ct,
                                      bool asNoTracking,
                                      string[]? includeNavigationNames)
    {
        return await _unitOfWork.Posts.GetAsync(p, ct, asNoTracking, includeNavigationNames);
    }

    public Post? Remove(Post entity)
    {
        return _unitOfWork.Posts.Remove(entity);
    }

    public Post? Update(Post entity)
    {
        return _unitOfWork.Posts.Update(entity);
    }

    public async Task<int> CompleteAsync() => await _unitOfWork.CompleteAsync();
}