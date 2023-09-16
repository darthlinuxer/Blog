namespace Application.Services;

public class PostService: IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PostModelDTO> _validator;

    public PostService(
        IUnitOfWork unitOfWork,
        IValidator<PostModelDTO> validator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<PostModel>> AddAsync(
        PostModelDTO entity)
    {
        var validPost = await _validator.ValidateAsync(entity);
        if (!validPost.IsValid) return Result<PostModel>.Failure(validPost.Errors.Select(c => c.ErrorMessage).ToList());

        var addedPost = await _unitOfWork.Posts.AddAsync(entity);
        if (addedPost is null) return Result<PostModel>.Failure(["Post not added!"]);
        await _unitOfWork.CompleteAsync();
        return Result<PostModel>.Success(addedPost);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllAsync(
        CancellationToken ct,
        string orderby = "PostId",
        int page = 1,
        int count = 10,
        bool descending = true,
        string[]? includeNavigationNames = null,
        bool asNoTracking = true)
    {
        return _unitOfWork.Posts.GetAllAsync(where: "PostId > 0",
                                             orderby,
                                             page,
                                             count,
                                             descending,
                                             includeNavigationNames,
                                             asNoTracking,
                                             ct);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllFilteredAsync(
        string where,
        CancellationToken ct,
        string orderby = "PostId",
        int page = 1,
        int count = 10,
        bool descending = true,
        string[]? includeNavigationNames = null,
        bool asNoTracking = true)
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

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorNameAsync(
        string authorname,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true
        )
    {
        return _unitOfWork.Posts.GetAllByAuthorNameAsync(authorname,
                                            ct,
                                            page: page,
                                            count: count,
                                            descending: descending,
                                            asNoTracking: asNoTracking);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorIdAsync(
      string authorId,
      CancellationToken ct,
      int page = 1,
      int count = 10,
      bool descending = true,
      bool asNoTracking = true,
      string[]? navigation = null)
    {
        return _unitOfWork.Posts.GetAllByAuthorIdAsync(authorId,
                                             ct,
                                             page: page,
                                             count: count,
                                             descending: descending,
                                             asNoTracking: asNoTracking,
                                             navigation);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByTitleAsync(
        string title,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null)
    {
        return _unitOfWork.Posts.GetAllByTitleAsync(title,
                                           ct,
                                           page: page,
                                           count: count,
                                           descending: descending,
                                           asNoTracking: asNoTracking,
                                           navigation);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByContentsAsync(
        string content,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null)
    {
        return _unitOfWork.Posts.GetAllByContentsAsync(content,
                                              ct,
                                              page: page,
                                              count: count,
                                              descending: descending,
                                              asNoTracking: asNoTracking,
                                              navigation);
    }

    public async Task<Result<PostModel>> GetAsync(
        Expression<Func<PostModel, bool>> p,
        CancellationToken ct,
        bool asNoTracking,
        string[]? includeNavigationNames)
    {
        var post = await _unitOfWork.Posts.GetAsync(p,
                                                    ct,
                                                    asNoTracking,
                                                    includeNavigationNames);
        if (post is null) return Result<PostModel>.Failure(["Post does not exist"]);
        return Result<PostModel>.Success(post);
    }

    public async Task<Result<PostModel>> UpdateAsync(
        PostModelDTO entity,
        CancellationToken ct)
    {
        try
        {
            var validPost = await _validator.ValidateAsync(entity);
            if (!validPost.IsValid) return Result<PostModel>.Failure(
                validPost.Errors.Select(c => c.ErrorMessage).ToList());

            var postInDb = await GetAsync(p => p.PostId == entity.PostId, ct, false, null);
            if (!postInDb.IsSuccess) return Result<PostModel>.Failure(postInDb.Errors);
            postInDb!.Value.Title = entity.Title;
            postInDb!.Value.Content = entity.Content;
            await _unitOfWork.CompleteAsync();
            return Result<PostModel>.Success(postInDb.Value);
        }
        catch (Exception ex)
        {
            return Result<PostModel>.Failure([ex.Message]);
        }
    }

    public async Task<Result<PostModel>> RemoveAsync(int postId, CancellationToken ct)
    {
        var post = await _unitOfWork.Posts.GetAsync(c => c.PostId == postId, ct, asNoTracking: true, includeNavigationNames: null);
        if (post is null) return Result<PostModel>.Failure([$"Post with id {postId} does not exist!"]);
        var removedPost = _unitOfWork.Posts.Remove(post);
        await _unitOfWork.CompleteAsync();
        return Result<PostModel>.Success(removedPost);
    }

    public bool Exist(Expression<Func<PostModel, bool>> p)
    {
        return _unitOfWork.Posts.Exist(p);
    }

    public int Count(Expression<Func<PostModel, bool>> p)
    {
        return _unitOfWork.Posts.Count(p);
    }
}