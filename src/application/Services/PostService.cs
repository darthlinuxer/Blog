namespace Application.Services;

public class PostService : IPostService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<PostModelDTO> _validator;
    private readonly IValidator<PostLifeCycle> _lifecycleValidator;

    public PostService(
        IUnitOfWork unitOfWork,
        IValidator<PostModelDTO> validator,
        IValidator<PostLifeCycle> lifecycleValidator)
    {
        _unitOfWork = unitOfWork;
        _validator = validator;
        this._lifecycleValidator = lifecycleValidator;
    }

    public async Task<Result<PostModel>> AddAsync(
        PostModelDTO entity)
    {
        var validPost = await _validator.ValidateAsync(entity);
        if (!validPost.IsValid) return Result<PostModel>.Failure(validPost.Errors.Select(c => c.ErrorMessage).ToList());

        var addedPost = await _unitOfWork.Posts.AddAsync(entity);
        if (addedPost is null) return Result<PostModel>.Failure(["Post not added!"]);
        await _unitOfWork.CompleteAsync();
        return Result<PostModel>.Success(entity);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllAsync(
        CancellationToken ct,
        string orderby = "Id",
        int page = 1,
        int count = 10,
        bool descending = true,
        string[]? includeNavigationNames = null,
        bool asNoTracking = true,
        Status postStatus = Status.published)
    {
        return _unitOfWork.Posts.GetAllAsync(ct: ct,
                                             orderBy: orderby,
                                             page: page,
                                             count: count,
                                             descending: descending,
                                             asNoTracking: asNoTracking,
                                             navigation: includeNavigationNames,
                                             postStatus: postStatus);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllFilteredAsync(string where, CancellationToken ct, string orderby = "Id", int page = 1, int count = 10, bool descending = true, string[]? includeNavigationNames = null, bool asNoTracking = true)
    {
        return _unitOfWork.Posts.GetAllFilteredAsync(where,
                                                     ct,
                                                     orderby,
                                                     page,
                                                     count,
                                                     descending,
                                                     asNoTracking,
                                                     includeNavigationNames);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorNameAsync(
        string authorname,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        Status status = Status.published)
    {
        return _unitOfWork.Posts.GetAllByAuthorNameAsync(authorname,
                                                         ct,
                                                         orderBy: orderBy,
                                                         page: page,
                                                         count: count,
                                                         descending: descending,
                                                         asNoTracking: asNoTracking,
                                                         status: status);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByAuthorIdAsync(
      string authorId,
      CancellationToken ct,
      string orderBy = "Title",
      int page = 1,
      int count = 10,
      bool descending = true,
      bool asNoTracking = true,
      string[]? navigation = null,
      Status postStatus = Status.published)
    {
        return _unitOfWork.Posts.GetAllByAuthorIdAsync(authorId,
                                                       ct,
                                                       orderBy: orderBy,
                                                       page: page,
                                                       count: count,
                                                       descending: descending,
                                                       asNoTracking: asNoTracking,
                                                       navigation: navigation,
                                                       postStatus: postStatus);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByTitleAsync(
        string title,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        Status postStatus = Status.published)
    {
        return _unitOfWork.Posts.GetAllByTitleAsync(title,
                                                    ct,
                                                    orderBy: orderBy,
                                                    page: page,
                                                    count: count,
                                                    descending: descending,
                                                    asNoTracking: asNoTracking,
                                                    navigation: navigation,
                                                    postStatus: postStatus);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel?> GetAllByContentsAsync(
        string content,
        CancellationToken ct,
        string orderBy = "Title",
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null,
        Status postStatus = Status.published)
    {
        return _unitOfWork.Posts.GetAllByContentsAsync(content,
                                                       ct,
                                                       orderBy: orderBy,
                                                       page: page,
                                                       count: count,
                                                       descending: descending,
                                                       asNoTracking: asNoTracking,
                                                       navigation: navigation,
                                                       postStatus: postStatus);
    }

    public async Task<Result<PostModel>> GetAsync(
        Expression<Func<PostModel, bool>> p,
        CancellationToken ct,
        bool asNoTracking = true,
        string[]? includeNavigationNames = null)
    {
        var post = await _unitOfWork.Posts.GetAsync(p: p,
                                                    ct: ct,
                                                    asNoTracking: asNoTracking,
                                                    includeNavigationNames: includeNavigationNames);
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

            var postInDb = await GetAsync(p: p => p.Id == entity.Id,
                                          ct: ct,
                                          asNoTracking: false,
                                          includeNavigationNames: null);
            if (!postInDb.IsSuccess) return Result<PostModel>.Failure(postInDb.Errors);
            postInDb!.Value.Title = entity.Title;
            postInDb!.Value.Content = entity.Content;
            postInDb!.Value.Status = entity.Status;
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
        var post = await _unitOfWork.Posts.GetAsync(p: c => c.Id == postId,
                                                    ct: ct,
                                                    asNoTracking: true,
                                                    includeNavigationNames: null);
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

    public async Task<Result<PostModel>> ChangePostStatus(ClaimsPrincipal principal, int postId, Status moveToStatus, CancellationToken ct)
    {
        var postInDb = await GetAsync(p: p => p.Id == postId,
                                        ct: ct,
                                        asNoTracking: false,
                                        includeNavigationNames: null);
        if (!postInDb.IsSuccess) return Result<PostModel>.Failure(postInDb.Errors);

        var authorId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var role = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Role)!.Value;

        var result = _lifecycleValidator.Validate(new PostLifeCycle()
        {
            PostId = postInDb.Value.Id,
            AuthorIdRequestingChange = authorId,
            Status = postInDb.Value.Status,
            MoveToStatus = moveToStatus,
            Role = role
        });

        if (!result.IsValid) return Result<PostModel>.Failure(result.Errors.Select(c => c.ErrorMessage).ToList());

        postInDb!.Value.Status = moveToStatus;
        await _unitOfWork.CompleteAsync();
        return Result<PostModel>.Success(postInDb.Value);
    }
}