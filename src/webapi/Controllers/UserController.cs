namespace WebApi.Controllers;

[ApiController]
[Route("api/credentials/")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(
        IUserService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("getall")]
    [Authorize("EditorPolicy")]
    public async IAsyncEnumerable<BlogUser?> GetAllAsync(
   [FromBody] PaginationRecord pagination,
   bool includePosts,
   [EnumeratorCancellation] CancellationToken ct)
    {
        var usersResult = _service.GetAll(page: pagination.page,
                                          count: pagination.count,
                                          orderby: c => c.UserName ?? "",
                                          descending: true,
                                          noTracking: true,
                                          includePosts: includePosts,
                                          ct);

        await foreach (var user in usersResult.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return user;
        }
    }

    [HttpPost]
    [Route("getallinrole")]
    [Authorize("EditorPolicy")]
    public async IAsyncEnumerable<BlogUser?> GetAllInRoleAsync(
        [FromBody] PaginationRecord pagination,
        string role,
        bool includePosts,
        [EnumeratorCancellation] CancellationToken ct)
    {
        var usersResult = _service.GetAllUsersByRole(
                                          role: role,
                                          page: pagination.page,
                                          count: pagination.count,
                                          orderby: c => c.UserName ?? "",
                                          descending: true,
                                          noTracking: true,
                                          includePosts: includePosts,
                                          ct);

        await foreach (var user in usersResult.WithCancellation(ct))
        {
            if (ct.IsCancellationRequested) break;
            yield return user;
        }
    }


}