using System.Runtime.CompilerServices;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/(controller)")]
public class PostController : ControllerBase
{
    private readonly IPostService _service;

    public PostController(IPostService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("(action)")]
    public async IAsyncEnumerable<IActionResult> GetAllPostsByAuthorAsync([AsParameters] PaginationRecord pagination,
                                                                          string author,
                                                                          [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllByAuthorAsync(author,
                                                 pagination.page,
                                                 pagination.count,
                                                 pagination.descending,
                                                 pagination.asNoTracking,
                                                 ["Author", "Comments"],
                                                 ct);
        if(!posts.HasValue) yield return Ok("There are no posts");
        await foreach(var post in posts){
            yield return Ok(post);
        }
    }

    [HttpGet]
    [Route("(action)")]
    public async IAsyncEnumerable<IActionResult> GetAllPostsFilteredAsync([AsParameters] PaginationRecord pagination,
                                                                          string where,
                                                                          string orderby,
                                                                          [EnumeratorCancellation] CancellationToken ct)
    {
        var posts = _service.GetAllAsync(where: where,
                                         orderby: orderby,
                                         page: pagination.page,
                                         count: pagination.count,
                                         descending: pagination.descending,
                                         ["Author","Comments"],
                                         asNoTracking: pagination.asNoTracking,
                                         ct: ct);
                                         
        if(!posts.HasValue) yield return Ok("There are no posts");
        await foreach(var post in posts){
            yield return Ok(post);
        }
    }
}