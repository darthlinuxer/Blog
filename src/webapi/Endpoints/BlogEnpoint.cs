namespace WebApi.Endpoints;

public static partial class EndpointExtensions
{
    public static void MapBlogEndpoints(this WebApplication app)
    {
        var blog = app.MapGroup("/api/blogs/");
        blog.MapGet("", () => "Get All Blogs");
        blog.MapGet("{id}", (int id) => $"Get a Blog with Id {id}");
        blog.MapPost("", () => "Post a new Blog");
        blog.MapPut("{id}", (int id) => $"Update a Blog with {id}");
        blog.MapDelete("{id}", (int id) => $"Delete a Blog with {id}");
    }
}