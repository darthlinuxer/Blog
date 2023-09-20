namespace WebApi.Endpoints;

public class PostMinimalApiEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/posts");
        group.MapGet("/{id:int}", GetWithIdAsync).WithName(nameof(GetWithIdAsync)).RequireAuthorization("PublicPolicy");
        group.MapPost("/", Post).WithName(nameof(Post)).RequireAuthorization("WriterPolicy");
        group.MapPut("/", Put).WithName(nameof(Put)).RequireAuthorization("WriterPolicy");
        group.MapDelete("/{id:int}", Delete).WithName(nameof(Delete)).RequireAuthorization("WriterPolicy");
        group.MapGet("/pending/{postId:int}", WriterChangeStatusToPending).WithName(nameof(WriterChangeStatusToPending)).RequireAuthorization("WriterPolicy");
        group.MapGet("/draft/{postId:int}", WriterChangeStatusToDraft).WithName(nameof(WriterChangeStatusToDraft)).RequireAuthorization("WriterPolicy");
        group.MapGet("/publish/{postId:int}", WriterChangeStatusToPublished).WithName(nameof(WriterChangeStatusToPublished)).RequireAuthorization("WriterPolicy");
        group.MapGet("/approve/{postId:int}", EditorChangeStatusToApproved).WithName(nameof(EditorChangeStatusToApproved)).RequireAuthorization("EditorPolicy");
        group.MapGet("/reject/{postId:int}", EditorChangeStatusToRejected).WithName(nameof(EditorChangeStatusToRejected)).RequireAuthorization("EditorPolicy");
    }

    public static async Task<IResult> WriterChangeStatusToDraft(
                   int postId,
                   [FromServices] IPostService service,
                   ClaimsPrincipal principal,
                   CancellationToken ct)
    {
        var postInDb = await service.ChangePostStatus(principal, postId, Status.draft, ct);
        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        return Results.Ok(postInDb.Value);
    }

    public static async Task<IResult> WriterChangeStatusToPending(
                   int postId,
                   [FromServices] IPostService service,
                   ClaimsPrincipal principal,
                   CancellationToken ct)
    {
        var postInDb = await service.ChangePostStatus(principal, postId, Status.pending, ct);
        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        return Results.Ok(postInDb.Value);
    }

    public static async Task<IResult> WriterChangeStatusToPublished(
                  int postId,
                  [FromServices] IPostService service,
                  ClaimsPrincipal principal,
                  CancellationToken ct)
    {
        var postInDb = await service.ChangePostStatus(principal, postId, Status.published, ct);
        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        return Results.Ok(postInDb.Value);
    }

    public static async Task<IResult> EditorChangeStatusToRejected(
                 int postId,
                 [FromServices] IPostService service,
                 ClaimsPrincipal principal,
                 CancellationToken ct)
    {
        var postInDb = await service.ChangePostStatus(principal, postId, Status.rejected, ct);
        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        return Results.Ok(postInDb.Value);
    }

    public static async Task<IResult> EditorChangeStatusToApproved(
                int postId,
                [FromServices] IPostService service,
                ClaimsPrincipal principal,
                CancellationToken ct)
    {
        var postInDb = await service.ChangePostStatus(principal, postId, Status.approved, ct);
        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        return Results.Ok(postInDb.Value);
    }


    public static async Task<IResult> GetWithIdAsync([FromRoute] int id,
                                                     [FromServices] IPostService service,
                                                     CancellationToken ct)
    {
        var postInDb = await service.GetAsync(c => c.Id == id && c.Status == Status.published,
                                          ct,
                                          asNoTracking: true,
                                          ["Author", "Comments"]);

        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        var options = new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.IgnoreCycles };
        return Results.Json(postInDb.Value, options);
    }

    public async Task<Result<PostModelDTO>> Post([FromBody] PostModelDTO postDTO,
                                    [FromServices] IPostService service,
                                    ClaimsPrincipal principal)
    {
        var loggedUserId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var postToAdd  = postDTO with {AuthorId = loggedUserId};
        var addedPost = await service.AddAsync(postToAdd);
        if (!addedPost.IsSuccess) return Result<PostModelDTO>.Failure(addedPost.Errors);

        postDTO = postDTO with {Id = addedPost.Value.Id};

        return Result<PostModelDTO>.Success(postDTO);
    }

    public async Task<IResult> Put([FromBody] PostModelDTO postDTO,
                                   [FromServices] IPostService service,
                                   CancellationToken ct)
    {
        var updatedPost = await service.UpdateAsync(postDTO, ct);
        if (!updatedPost.IsSuccess) return Results.BadRequest(updatedPost.Errors);
        return Results.Ok(updatedPost.Value);
    }

    public async Task<IResult> Delete([FromRoute] int id,
                                      [FromServices] IPostService service,
                                      CancellationToken ct)
    {
        var removePost = await service.RemoveAsync(id, ct);
        if (!removePost.IsSuccess) return Results.BadRequest(removePost.Errors);
        return Results.Ok(removePost.Value);
    }
}
