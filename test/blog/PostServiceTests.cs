namespace blog;

[TestClass]
public class PostServiceTests
{
    private IPostService _postService;
    private IUserService _userService;

    public PostServiceTests()
    {
        _postService = SharedSetupFixture.PostService;
        _userService = SharedSetupFixture.UserService;
    }

    [TestInitialize]
    public void Seed() => SharedSetupFixture.SeedData();

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
        var loginResult = await _userService.LoginAsync("luke", "ChangeMe1$");
        var token = loginResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var loggedUserId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;

        //Act
        var postResult = await _postService.AddAsync(new PostModelDTO()
        {
            AuthorId = loggedUserId,
            Title = "Test",
            Content = "Test"
        });

        //Result
        Assert.IsFalse(postResult.IsSuccess);
        Assert.IsTrue(postResult.Errors.Contains($"Author with Id {loggedUserId} is not a Writer!"));
    }

    [TestMethod]
    public async Task WriterUser_CanAddPosts()
    {
        //Arrange
        var loginResult = await _userService.LoginAsync("darthlinuxer", "ChangeMe1$");
        var token = loginResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var loggedUserId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value;

        //Act
        var post1Result = await _postService.AddAsync(new PostModelDTO()
        {
            AuthorId = loggedUserId,
            Title = "Test1",
            Content = "Test1"
        });

        var post2Result = await _postService.AddAsync(new PostModelDTO()
        {
            AuthorId = loggedUserId,
            Title = "Test2",
            Content = "Test2"
        });

        //Result
        Assert.IsTrue(post1Result.IsSuccess);
        Assert.IsTrue(post2Result.IsSuccess);
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
            AuthorId = post1Result.Value.AuthorId
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