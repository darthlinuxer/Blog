namespace WebApi.Endpoints;

public class PostEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/posts").RequireAuthorization();
        app.MapGet("/{id}", Get).WithName(nameof(Get));
        app.MapPost("", Post).WithName(nameof(Post));
        app.MapPut("/{id}", Put).WithName(nameof(Put));
        app.MapDelete("/{id}", Delete).WithName(nameof(Delete));

    }

    public static async Task<IResult> Get([FromRoute] int id,
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

    public async Task<IResult> Post([FromBody] PostModelDTO postDTO,
                                    [FromServices] IPostService service,
                                    [FromServices] IValidator<PostModelDTO> validator)
    {
        var validationResult = await validator.ValidateAsync(postDTO);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var addedPost = await service.AddAsync(postDTO);
        if (!addedPost.IsSuccess) return Results.BadRequest(addedPost.Errors);
        return Results.Ok(addedPost.Value);
    }

    public async Task<IResult> Put([FromRoute] int id,
                                   [FromBody] PostModelDTO postDTO,
                                   [FromServices] IPostService service,
                                   [FromServices] IValidator<PostModelDTO> validator,
                                   CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(postDTO);
        if (!validationResult.IsValid)
            return Results.BadRequest(validationResult.Errors);

        var postInDb = await service.GetAsync(c => c.PostId == id,
                                          ct,
                                          asNoTracking: true,
                                          ["Author", "Comments"]);

        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        var updatedPost = service.Update(postDTO);
        if (!updatedPost.IsSuccess) return Results.BadRequest(updatedPost.Errors);
        await service.CompleteAsync();
        return Results.Ok(updatedPost.Value);
    }

    public async Task<IResult> Delete([FromRoute] int id,
                                      [FromServices] IPostService service,
                                      CancellationToken ct)
    {
        var postInDb = await service.GetAsync(c => c.PostId == id,
                                          ct,
                                          asNoTracking: true,
                                          ["Author", "Comments"]);

        if (!postInDb.IsSuccess) return Results.BadRequest(postInDb.Errors);
        var removePost = service.Remove(postInDb.Value);
        if (!removePost.IsSuccess) return Results.BadRequest(removePost.Errors);
        await service.CompleteAsync();
        return Results.Ok(removePost.Value);
    }
}
