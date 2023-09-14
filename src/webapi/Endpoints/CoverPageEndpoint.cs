using System.Net.Mime;

namespace WebApi.Endpoints;

public class CoverPageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", GetCoverPage).WithName(nameof(GetCoverPage));
    }

    public void GetCoverPage(HttpContext context){
        var html = @"
        <html>
            <body>
                <h1>Welcome to the Blog WebApi</h1>
                <br/>
                <p>Go to /swagger to see the endpoints
                or open import the postman collection on Git</p>
            </body>
        </html>
        ";

        WriteHtml(context, html);

    }

    private void WriteHtml(HttpContext context, string html){
        context.Response.ContentType = MediaTypeNames.Text.Html;
        context.Response.ContentLength = Encoding.UTF8.GetByteCount(html);
        context.Response.WriteAsync(html);
    }
}