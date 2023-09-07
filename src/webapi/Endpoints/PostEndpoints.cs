namespace WebApi.Endpoints;

public static partial class EndpointExtensions
{
    public static void MapPostEndpoints(this WebApplication app)
    {
        var post = app.MapGroup("/api/posts/");

        post.MapGet("{id}", async (int id,
                                   CancellationToken ct,
                                   PostService service) => await service.GetAsync(c => c.PostId == id,
                                                                                  ct,
                                                                                  asNoTracking: true,
                                                                                  ["Author", "Comments"]));

        post.MapPost("", async (Post post,
                                PostService service) => await service.AddAsync(post));

        post.MapPut("{id}", async (int id,
                                   Post post,
                                   CancellationToken ct,
                                   PostService service) =>
        {
            var postInDb = await service.GetAsync(p => p.PostId == id,
                                                  ct,
                                                  asNoTracking: false,
                                                  ["Author", "Comments"]);
            return postInDb;
        });

        post.MapDelete("{id}", async (int id,
                                      CancellationToken ct,
                                      PostService service) =>
        {
            var postInDb = await service.GetAsync(p => p.PostId == id,
                                                  ct,
                                                  asNoTracking: true,
                                                  null);
            return service.Remove(postInDb);
        });
    }
}