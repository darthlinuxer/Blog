using Microsoft.AspNetCore.Http;
using Moq;

namespace blog;

[TestClass]
public class PostServiceTests
{
    private IPostService _postService;
    private IUserService _userService;
    private SharedSetup setup;
    private ServiceProvider _serviceProvider;

    public PostServiceTests()
    {
        setup = new SharedSetup();
        _postService = setup.PostService;
        _userService = setup.UserService;
        _serviceProvider = setup.ServiceProvider;
    }

    [TestInitialize]
    public void Seed() => setup.SeedData();

    [TestMethod]
    public async Task GetAllPostsWithEmptyDb_ShouldReturnNothing()
    {
        var ct = new CancellationToken();
        var posts = _postService.GetAllAsync(ct);
        var count = 0;
        await foreach (var post in posts.WithCancellation(ct))
        {
            count++;
        }
        Assert.IsTrue(count == 0);
    }

    [TestMethod]
    public async Task PublicUser_CannotAddPosts()
    {
        //Arrange
        //Luke is a public user and cannot add posts
        var loginResult = await _userService.LoginAsync("luke", "ChangeMe1$");
        var token = loginResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var loggedUserId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);

        // Act
        var authorizationService = _serviceProvider.GetRequiredService<IAuthorizationService>();
        var authResult = await authorizationService.AuthorizeAsync(principal, null, "WriterPolicy");

        //Result
        Assert.IsFalse(authResult.Succeeded);
    }

    [TestMethod]
    public async Task WriterUser_CanAddPosts()
    {
        //Arrange
        //darthlinuxer is a writer and cannot add posts
        var loginResult = await _userService.LoginAsync("darthlinuxer", "ChangeMe1$");
        var token = loginResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);

        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns(principal);

        // Act
        var authorizationService = _serviceProvider.GetRequiredService<IAuthorizationService>();
        var authResult = await authorizationService.AuthorizeAsync(principal, null, "WriterPolicy");

        //Result
        Assert.IsTrue(authResult.Succeeded);
    }

    [TestMethod]
    public async Task WriterUserModifyPostTitleAndComment_PostShouldUpdate()
    {
        //Arrange
        var loginResult = await _userService.LoginAsync("darthlinuxer", "ChangeMe1$");
        var token = loginResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var loggedUserId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;

        //Act
        var post1Result = await _postService.AddAsync(new PostModelDTO()
        {
            AuthorId = loggedUserId,
            Title = "Test1",
            Content = "Test1"
        });

        var newData = new PostModelDTO()
        {
            PostId = post1Result.Value.PostId,
            Title = "New Test",
            Content = "New Content",
            AuthorId = loggedUserId
        };

        var updatedPost = await _postService.UpdateAsync(newData, CancellationToken.None);

        //Result
        Assert.IsTrue(post1Result.IsSuccess);
        Assert.IsTrue(updatedPost.Value.Title == "New Test");
        Assert.IsTrue(updatedPost.Value.Content == "New Content");
        Assert.IsTrue(updatedPost.Value.PostId == post1Result.Value.PostId);
        Assert.IsTrue(updatedPost.Value.AuthorId == post1Result.Value.AuthorId);
    }
}