
using System.Drawing;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Application.Services;

public class CommentService: ICommentService
{
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

     public async Task<Result<Comment>> AddAsync(Comment entity)
    {
        var addedComment = await _unitOfWork.Comments.AddAsync(entity);
        if(addedComment is null) return Result<Comment>.Failure(["Comment not added"]);
        return Result<Comment>.Success(addedComment);
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

    public async Task<Result<Comment>> GetAsync(Expression<Func<Comment, bool>> p,
                                   CancellationToken ct,
                                   bool asNoTracking,
                                   string[]? includeNavigationNames)
    {
        var comment = await _unitOfWork.Comments.GetAsync(p, ct, asNoTracking, includeNavigationNames);
        if(comment is null) return Result<Comment>.Failure([""]);
        return Result<Comment>.Success(comment);
    }

    public Result<Comment> Remove(Comment entity)
    {
        var comment = _unitOfWork.Comments.Remove(entity);
        if(comment is null) return Result<Comment>.Failure(["Comment not found!"]);
        return Result<Comment>.Success(comment);
    }

    public Result<Comment> Update(Comment entity)
    {
        var comment = _unitOfWork.Comments.Update(entity);
         if(comment is null) return Result<Comment>.Failure(["Comment not found!"]);
        return Result<Comment>.Success(comment);
    }

    public async Task<int> CompleteAsync() => await _unitOfWork.CompleteAsync();
}