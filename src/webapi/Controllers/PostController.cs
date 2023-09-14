using Microsoft.Extensions.Hosting;
using System.IO.Compression;
using System.Net.Mime;

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
    public async IAsyncEnumerable<IActionResult> GetMyPostsAsync(
     [FromBody] PaginationRecord pagination,
     [EnumeratorCancellation] CancellationToken ct)
    {
        var author = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        var postsAsync = _service.GetAllByAuthorAsync(author,
                                                 ct,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 null
                                                 );
        var count = 0;
        await foreach (var post in postsAsync.WithCancellation(ct))
        {
            count++;
            if (ct.IsCancellationRequested) break;
            yield return Ok(post);
        }
        if (count == 0) yield return Ok("There are no posts");
    }

    [HttpGet]
    [Route("getallpostsbyauthor")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<IActionResult> GetAllPostsByAuthorAsync(
       [AsParameters] PaginationRecord pagination,
       [FromQuery] string author,
       [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllByAuthorAsync(
                                                 author,
                                                 ct,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"]
                                                 );
        await foreach (var post in posts.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return Ok(post);
        }
    }

    [HttpGet]
    [Route("getallpostsfiltered")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<IActionResult> GetAllPostsFilteredAsync(
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
            yield return Ok(post);
        }
    }

    [HttpGet]
    [Route("getallposts")]
    [Authorize("PublicPolicy")]
    public async IAsyncEnumerable<IActionResult> GetAllPostsAsync(
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
            yield return Ok(post);
        }
    }
}