namespace WebApi.Controllers;

[ApiController]
[Route("api/comments/")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _service;

    public CommentsController(ICommentService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("getallmycommments")]
    [Authorize("WriterPolicy")]
    public async IAsyncEnumerable<Comment?> GetAllMyCommentsAsync(
   [FromBody] PaginationRecord pagination,
   [EnumeratorCancellation] CancellationToken ct)
    {
        var authorName = HttpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        var commentsAsync = _service.GetAllCommentsOfUserAsync(
            authorName,
            pagination.page,
            pagination.count,
            pagination.descending,
            true,
            ["Author", "Comments"],
            ct);

        await foreach (var comment in commentsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return comment;
        }
    }

    [HttpPost]
    [Route("getallcommentsbyuser")]
    [Authorize("EditorPolicy")]
    public async IAsyncEnumerable<Comment?> GetAllCommentsByUserAsync(
   [FromBody] PaginationRecord pagination,
   string authorName,
   [EnumeratorCancellation] CancellationToken ct)
    {
        var commentsAsync = _service.GetAllCommentsOfUserAsync(
            authorName,
            pagination.page,
            pagination.count,
            pagination.descending,
            true,
            ["Author", "Comments"],
            ct);

        await foreach (var comment in commentsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return comment;
        }
    }

    [HttpPost]
    [Route("getallcommentsofpost")]
    [Authorize("EditorPolicy")]
    public async IAsyncEnumerable<Comment?> GetAllCommentsOfPostAsync(
        [FromBody] PaginationRecord pagination,
        int postId,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var commentsAsync = _service.GetAllCommentsForPostAsync(
                                                 postId,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 true,
                                                 ["Author", "Comments"],
                                                 ct);

        await foreach (var comment in commentsAsync.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return comment;
        }
    }
}