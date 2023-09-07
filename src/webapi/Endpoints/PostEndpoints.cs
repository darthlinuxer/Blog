using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

public static partial class EndpointExtensions
{
    public static void MapPostEndpoints(this WebApplication app)
    {
        var post = app.MapGroup("/api/posts/");

        post.MapGet("{id}", async ([FromRoute] int id,
                                   [FromServices] PostService service,
                                   CancellationToken ct) => 
                                   await service.GetAsync(c => c.PostId == id,
                                                                                  ct,
                                                                                  asNoTracking: true,
                                                                                  ["Author", "Comments"]));

        post.MapPost("", async ([FromBody] Post post,
                                [FromServices] PostService service) => 
                                await service.AddAsync(post));

        post.MapPut("{id}", async ([FromRoute] int id,
                                   [FromBody] Post post,
                                   [FromServices] PostService service,
                                   CancellationToken ct) =>
        {
            var postInDb = await service.GetAsync(p => p.PostId == id,
                                                  ct,
                                                  asNoTracking: false,
                                                  ["Author", "Comments"]);
            return postInDb;
        });

        post.MapDelete("{id}", async ([FromRoute] int id,
                                      [FromServices] PostService service,
                                      CancellationToken ct) =>
        {
            var postInDb = await service.GetAsync(p => p.PostId == id,
                                                  ct,
                                                  asNoTracking: true,
                                                  null);
            return service.Remove(postInDb);
        });
    }
}