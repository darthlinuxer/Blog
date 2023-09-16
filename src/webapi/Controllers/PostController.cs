namespace WebApi.Controllers;

[ApiController]
[Route("api/posts/")]
public class PostsController : ControllerBase
{
    private readonly IPostService _service;

    public PostsController(IPostService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("getallmyposts")]
    [Authorize("WriterPolicy")]
    public async IAsyncEnumerable<PostModel> GetAllMyPostsAsync(
   [FromBody] PaginationRecord pagination,
   [EnumeratorCancellation] CancellationToken ct)
    {
        var authorId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var postsAsync = _service.GetAllFilteredAsync(
                                                 $"AuthorId == \"{authorId}\"",
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 ["Author", "Comments"],
                                                 pagination.asNoTracking
                                                 );
        await foreach (var post in postsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getmypublishedposts")]
    [Authorize("WriterPolicy")]
    public async IAsyncEnumerable<PostModel> GetAllMyPublishedPostsAsync(
     [FromBody] PaginationRecord pagination,
     [EnumeratorCancellation] CancellationToken ct)
    {
        var authorId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var postsAsync = _service.GetAllByAuthorIdAsync(authorId,
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"],
                                                 postStatus: PostStatus.published
                                                 );
        await foreach (var post in postsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getmydraftposts")]
    [Authorize("WriterPolicy")]
    public async IAsyncEnumerable<PostModel> GetMyDraftPostsAsync(
     [FromBody] PaginationRecord pagination,
     [EnumeratorCancellation] CancellationToken ct)
    {
        var authorId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var postsAsync = _service.GetAllByAuthorIdAsync(authorId,
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"],
                                                 postStatus: PostStatus.draft
                                                 );
        await foreach (var post in postsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getmyrejectedposts")]
    [Authorize("WriterPolicy")]
    public async IAsyncEnumerable<PostModel> GetMyRejectedPostsAsync(
    [FromBody] PaginationRecord pagination,
    [EnumeratorCancellation] CancellationToken ct)
    {
        var authorId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var postsAsync = _service.GetAllByAuthorIdAsync(authorId,
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"],
                                                 postStatus: PostStatus.rejected
                                                 );
        await foreach (var post in postsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getmypendingposts")]
    [Authorize("WriterPolicy")]
    public async IAsyncEnumerable<PostModel> GetMyPendingPostsAsync(
    [FromBody] PaginationRecord pagination,
    [EnumeratorCancellation] CancellationToken ct)
    {
        var authorId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var postsAsync = _service.GetAllByAuthorIdAsync(authorId,
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"],
                                                 postStatus: PostStatus.pending
                                                 );
        await foreach (var post in postsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getmyapprovedposts")]
    [Authorize("WriterPolicy")]
    public async IAsyncEnumerable<PostModel> GetMyApprovedPostsAsync(
   [FromBody] PaginationRecord pagination,
   [EnumeratorCancellation] CancellationToken ct)
    {
        var authorId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var postsAsync = _service.GetAllByAuthorIdAsync(authorId,
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"],
                                                 postStatus: PostStatus.approved
                                                 );
        await foreach (var post in postsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getallpostsbyauthor")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<PostModel> GetAllPublishedPostsByAuthorAsync(
       [AsParameters] PaginationRecord pagination,
       [FromQuery] string author,
       [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllByAuthorNameAsync(
                                                 author,
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 PostStatus.published
                                                 );
        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getallpostsbytitle")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<PostModel> GetAllPublishedPostsByTitleAsync(
       [AsParameters] PaginationRecord pagination,
       [FromQuery] string title,
       [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllByTitleAsync(
                                                 title,
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"],
                                                 PostStatus.published
                                                 );
        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getallpostsbycontent")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<PostModel> GetAllPublishedPostsByContentAsync(
     [AsParameters] PaginationRecord pagination,
     [FromQuery] string content,
     [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllByContentsAsync(
                                                 content,
                                                 ct,
                                                 pagination.orderby,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"],
                                                 PostStatus.published
                                                 );
        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("editorgetallpostsfiltered")]
    [Authorize("EditorPolicy")]
    public async IAsyncEnumerable<PostModel> GetAllPostsFilteredAsync(
        [FromBody] PaginationRecord pagination,
        [FromQuery] string where,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllFilteredAsync(where: where,
                                         ct: ct,
                                         orderby: pagination.orderby,
                                         page: pagination.page,
                                         count: pagination.count,
                                         descending: pagination.descending,
                                         ["Author", "Comments"],
                                         asNoTracking: pagination.asNoTracking);

        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

     [HttpPost]
    [Route("editorgetallpostsbyauthorandstatus")]
    [Authorize("EditorPolicy")]
    public async IAsyncEnumerable<PostModel?> GetAllPostsByAuthorAndStatusAsync(
        [FromBody] PaginationRecord pagination,
        [FromQuery] string author,
        [FromQuery] string status,
        [EnumeratorCancellation] CancellationToken ct)
    {
        PostStatus postStatus;
        if (!Enum.TryParse(status, out postStatus)) yield return null;
        var posts = _service.GetAllByAuthorNameAsync(
            author,
            ct: ct,
            orderBy: pagination.orderby,
            page: pagination.page,
            count: pagination.count,
            descending: pagination.descending,
            asNoTracking: pagination.asNoTracking,
            postStatus: postStatus);

        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getallpublishedposts")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<PostModel?> GetAllPublishedPostsAsync(
        [FromBody] PaginationRecord pagination,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllAsync(ct,
                                         orderby: pagination.orderby,
                                         page: pagination.page,
                                         count: pagination.count,
                                         descending: pagination.descending,
                                         includeNavigationNames: ["Author", "Comments"],
                                         asNoTracking: pagination.asNoTracking,
                                         postStatus:PostStatus.published
                                         );

        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }
}