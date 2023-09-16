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
    [Route("getmyposts")]
    [Authorize("WriterPolicy")]
    public async IAsyncEnumerable<PostModel> GetMyPostsAsync(
     [FromBody] PaginationRecord pagination,
     [EnumeratorCancellation] CancellationToken ct)
    {
        var authorId = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var postsAsync = _service.GetAllByAuthorIdAsync(authorId,
                                                 ct,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"]
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
    public async IAsyncEnumerable<PostModel> GetAllPostsByAuthorAsync(
       [AsParameters] PaginationRecord pagination,
       [FromQuery] string author,
       [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllByAuthorNameAsync(
                                                 author,
                                                 ct,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking
                                                 );
        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }

    [HttpPost]
    [Route("getallpostsfiltered")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<PostModel> GetAllPostsFilteredAsync(
        [AsParameters] PaginationRecord pagination,
        [EnumeratorCancellation] CancellationToken ct,
        [FromQuery] string where = "PostId > 0",
        [FromQuery] string orderby = "Title")
    {
        var posts = _service.GetAllFilteredAsync(where: where,
                                         ct: ct,
                                         orderby: orderby,
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
    [Route("getallposts")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<PostModel> GetAllPostsAsync(
       [EnumeratorCancellation] CancellationToken ct,
       [AsParameters] PaginationRecord pagination,
       [FromQuery] string orderby = "Title")
    {
        var posts = _service.GetAllAsync(ct,
                                         orderby: orderby,
                                         page: pagination.page,
                                         count: pagination.count,
                                         descending: pagination.descending,
                                         ["Author", "Comments"],
                                         asNoTracking: pagination.asNoTracking
                                         );

        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return post;
        }
    }
}