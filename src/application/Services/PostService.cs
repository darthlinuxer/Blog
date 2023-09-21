namespace Application.Services;

public class PostService
{
    private readonly IUnitOfWork _unitOfWork;
    //private readonly IValidator<PostModelDTO> _validator;
    private readonly IValidator<PostLifeCycle> _lifecycleValidator;

    public PostService(
        IUnitOfWork unitOfWork,
        //IValidator<PostModelDTO> validator,
        IValidator<PostLifeCycle> lifecycleValidator)
    {
        _unitOfWork = unitOfWork;
        //_validator = validator;
        this._lifecycleValidator = lifecycleValidator;
    }

    public async Task<Result<PostModel>> AddAsync(
        PostModel entity)
    {
        //var validPost = await _validator.ValidateAsync(entity);
        //if (!validPost.IsValid) return Result<PostModel>.Failure(validPost.Errors.Select(c => /c.ErrorMessage).ToList());

        await _unitOfWork.Articles.AddAsync(entity);
        await _unitOfWork.CompleteAsync();
        return Result<PostModel>.Success(entity);
    }

    public ConfiguredCancelableAsyncEnumerable<PostModel>? GetAllAsync(
        CancellationToken ct,
        string orderby = "Id",
        int page = 1,
        int count = 10,
        bool descending = true,
        string[]? includeNavigationNames = null,
        bool asNoTracking = true,
        Status postStatus = Status.published)
    {
        return _unitOfWork.Articles.GetAllAsync<PostModel>(
            where: "Id>0 && Status == \"{postStatus}\"",
            orderby: orderby,
            page: page,
            count: count,
            descending: descending,
            includeNavigationNames: includeNavigationNames,
            asNoTracking: asNoTracking,
            ct);
    }

    public async Task<Result<PostModel>> GetAsync(
        Expression<Func<PostModel, bool>> p,
        CancellationToken ct,
        bool asNoTracking = true,
        string[]? includeNavigationNames = null)
    {
        var post = await _unitOfWork.Articles.GetAsync(p: p,
                                                    ct: ct,
                                                    asNoTracking: asNoTracking,
                                                    includeNavigationNames: includeNavigationNames);
        if (post is null) return Result<PostModel>.Failure(["Post does not exist"]);
        return Result<PostModel>.Success(post);
    }

    public async Task<Result<PostModel>> UpdateAsync(
        object entity,
        CancellationToken ct)
    {
        try
        {
            /*var validPost = await _validator.ValidateAsync(entity);
            if (!validPost.IsValid) return Result<PostModel>.Failure(
                validPost.Errors.Select(c => c.ErrorMessage).ToList());
            */

            var postInDb = await GetAsync(p: p => p.Id == (entity as PostModel).Id,
                                          ct: ct,
                                          asNoTracking: false,
                                          includeNavigationNames: null);
            if (!postInDb.IsSuccess) return Result<PostModel>.Failure(postInDb.Errors);
            postInDb!.Value.Title = (entity as PostModel).Title;
            postInDb!.Value.Content = (entity as PostModel).Content;
            postInDb!.Value.ArticleStatus = (entity as PostModel).ArticleStatus;
            await _unitOfWork.CompleteAsync();
            return Result<PostModel>.Success(postInDb.Value);
        }
        catch (Exception ex)
        {
            return Result<PostModel>.Failure([ex.Message]);
        }
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
            Status = postInDb.Value.ArticleStatus,
            MoveToStatus = moveToStatus,
            Role = role
        });

        if (!result.IsValid) return Result<PostModel>.Failure(result.Errors.Select(c => c.ErrorMessage).ToList());

        postInDb!.Value.ArticleStatus = moveToStatus;
        await _unitOfWork.CompleteAsync();
        return Result<PostModel>.Success(postInDb.Value);
    }
}