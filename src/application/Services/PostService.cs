namespace Application.Services;

public class PostService : IPostService
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

    public ConfiguredCancelableAsyncEnumerable<PostModel> GetAllAsync(
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

    public ConfiguredCancelableAsyncEnumerable<PostModel> GetAllFilteredAsync(
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

    public ConfiguredCancelableAsyncEnumerable<PostModel> GetAllByAuthorAsync(
        string author,
        CancellationToken ct,
        int page = 1,
        int count = 10,
        bool descending = true,
        bool asNoTracking = true,
        string[]? navigation = null
        )
    {
        return _unitOfWork.Posts.GetAllByAuthorAsync(author,
                                                     page,
                                                     count,
                                                     descending,
                                                     asNoTracking,
                                                     navigation,
                                                     ct);
    }

    public async Task<Result<PostModel>> GetAsync(
        Expression<Func<PostModel, bool>> p,
        CancellationToken ct,
        bool asNoTracking,
        string[]? includeNavigationNames)
    {
        var post = await _unitOfWork.Posts.GetAsync(p, ct, asNoTracking, includeNavigationNames);
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

            var postInDbResult = await GetAsync(
                                c => c.PostId == entity.PostId,
                                ct,
                                asNoTracking: true,
                                includeNavigationNames: null);

            if (ct.IsCancellationRequested) return Result<PostModel>.Failure(["Cancellation token called!"]);
            if (!postInDbResult.IsSuccess) return Result<PostModel>.Failure([$"Post with Id {entity.PostId} does not exist!"]);

            if (postInDbResult.Value.AuthorId != entity.AuthorId) return Result<PostModel>.Failure([$"User {entity.AuthorId} is not the owner of this Post being updated!"]);
            //map the contents to update
            postInDbResult.Value.Title = entity.Title;
            postInDbResult.Value.Content = entity.Content;

            var updatedPost = _unitOfWork.Posts.Update(postInDbResult.Value);
            await _unitOfWork.CompleteAsync();
            return Result<PostModel>.Success(updatedPost);
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
}