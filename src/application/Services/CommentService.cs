
using System.Runtime.CompilerServices;

namespace Application.Services;

public class CommentService: ICommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    async Task<Comment?> IGenericRepository<Comment>.AddAsync(Comment entity)
    {
        return await _unitOfWork.Comments.AddAsync(entity);
    }

    public ConfiguredCancelableAsyncEnumerable<Comment>? GetAllAsync(string where,
                                                                     string orderby,
                                                                     int page,
                                                                     int count,
                                                                     bool descending,
                                                                     string[]? includeNavigationNames,
                                                                     bool asNoTracking, 
                                                                     CancellationToken ct)
    {
        return _unitOfWork.Comments.GetAllAsync(where,
                                                orderby,
                                                page,
                                                count,
                                                descending,
                                                includeNavigationNames,
                                                asNoTracking,
                                                ct);
    }

    public ConfiguredCancelableAsyncEnumerable<Comment>? GetAllCommentsForPostAsync(int postId,
                                                                                    int page,
                                                                                    int count,
                                                                                    bool descending,
                                                                                    bool asNoTracking,
                                                                                    string[]? navigation,
                                                                                    CancellationToken ct)
    {
        return _unitOfWork.Comments.GetAllCommentsForPostAsync(postId,
                                                               page,
                                                               count,
                                                               descending,
                                                               asNoTracking,
                                                               navigation,
                                                               ct);
    }

    public ConfiguredCancelableAsyncEnumerable<Comment>? GetAllCommentsOfUserAsync(string username,
                                                                                   int page,
                                                                                   int count,
                                                                                   bool descending,
                                                                                   bool asNoTracking,
                                                                                   string[]? navigation,
                                                                                   CancellationToken ct)
    {
        return _unitOfWork.Comments.GetAllCommentsOfUserAsync(username,
                                                              page,
                                                              count,
                                                              descending,
                                                              asNoTracking,
                                                              navigation,
                                                              ct);
    }

    public Task<Comment?> GetAsync(Expression<Func<Comment, bool>> p,
                                   CancellationToken ct,
                                   bool asNoTracking,
                                   string[]? includeNavigationNames)
    {
        return _unitOfWork.Comments.GetAsync(p, ct, asNoTracking, includeNavigationNames);
    }

    public Comment? Remove(Comment entity)
    {
        return _unitOfWork.Comments.Remove(entity);
    }

    public Comment? Update(Comment entity)
    {
        return _unitOfWork.Comments.Update(entity);
    }

    public async Task<int> CompleteAsync() => await _unitOfWork.CompleteAsync();
}