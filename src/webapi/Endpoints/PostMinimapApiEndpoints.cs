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

    }

    public static async Task<IResult> GetWithIdAsync([FromRoute] int id,
                                                     [FromServices] IPostService service,
                                                     CancellationToken ct)
    {
        var postInDb = await service.GetAsync(c => c.PostId == id,
                                          ct,
                                          asNoTracking: true,
                                          ["Author", "Comments"]);

        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        return Results.Ok(postInDb.Value);
    }

    public async Task<Result<PostModelDTO>> Post([FromBody] PostModelDTO postDTO,
                                    [FromServices] IPostService service,
                                    ClaimsPrincipal principal)
    {
        var loggedUserId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        postDTO.AuthorId = loggedUserId;
        var addedPost = await service.AddAsync(postDTO);
        if (!addedPost.IsSuccess) return Result<PostModelDTO>.Failure(addedPost.Errors);

        var postAdded = new PostModelDTO{
            PostId = addedPost.Value.PostId,
            Title = addedPost.Value.Title,
            Content = addedPost.Value.Content,
            DatePublished = addedPost.Value.DatePublished,
            AuthorId = addedPost.Value.AuthorId
        };
        return Result<PostModelDTO>.Success(postAdded);
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
