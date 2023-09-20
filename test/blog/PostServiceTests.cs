using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Moq;

namespace blog;

[TestClass]
public class PostServiceTests
{
    private IPostService _postService;
    private IPersonService<Person> _userService;
    private SharedSetup setup;
    private ServiceProvider _serviceProvider;

    public PostServiceTests()
    {
        setup = new SharedSetup();
        _postService = setup.PostService;
        _userService = setup.PersonService;
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
        var post1Result = await _postService.AddAsync(new PostModelDTO(
            authorId: loggedUserId,
            title: "Test1",
            content: "Test1"
        ));

        var newData = new PostModelDTO(
            loggedUserId,
            "New Title",
            "New Content"
        ) with
        { Id = post1Result.Value.Id };

        var updatedPost = await _postService.UpdateAsync(newData, CancellationToken.None);

        //Result
        Assert.IsTrue(post1Result.IsSuccess);
        Assert.IsTrue(updatedPost.IsSuccess);
        Assert.IsTrue(updatedPost.Value.Title == newData.Title);
        Assert.IsTrue(updatedPost.Value.Content == newData.Content);
        Assert.IsTrue(updatedPost.Value.Status == Status.published);
        Assert.IsTrue(updatedPost.Value.Id == post1Result.Value.Id);
        Assert.IsTrue(updatedPost.Value.AuthorId == post1Result.Value.AuthorId);
    }

    [TestMethod]
    public async Task GetAllPostsFromWriter_ShouldReturnPosts()
    {
        //Arrange
        //Add another writer
        var user = await _userService.RegisterAsync(new UserRecordDTO(
            "Palpatine",
            "ChangeMe1$",
            "emperor@deathstar.com",
            "Writer"
        ));

        var darthLinuxerLoggedResult = await _userService.LoginAsync("darthlinuxer", "ChangeMe1$");
        var token = darthLinuxerLoggedResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var darthLinuxerId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var darthlinuxerName = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

        //Act
        //Adding 2 posts from Writer DarthLinuxer
        var post1Result = await _postService.AddAsync(new PostModelDTO(
            darthLinuxerId,
            "Test1",
            "Content1"
        ));
        //PostStatus is hidden as draft

        var post2Result = await _postService.AddAsync(new PostModelDTO(
            darthLinuxerId,
            "Test2",
            "Content2"
        ));

        var palpatineLoggedResult = await _userService.LoginAsync("palpatine", "ChangeMe1$");
        token = palpatineLoggedResult.Value;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var palpatineId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var palpatineName = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

        var post3Result = await _postService.AddAsync(new PostModelDTO(
            palpatineId,
            "Test3",
            "Content3"));

        var ct = new CancellationToken();
        var palpatinePosts = _postService.GetAllByAuthorNameAsync(palpatineName, ct, "Title", 1, 10, true, true, Status.draft);
        List<PostModel> palpatinePostsInDb = new();
        await foreach (var post in palpatinePosts.WithCancellation(ct))
        {
            palpatinePostsInDb.Add(post);
        }

        var allPosts = _postService.GetAllAsync(ct, postStatus: Status.draft);
        var allPostsInDb = new List<PostModel>();
        await foreach (var post in allPosts.WithCancellation(ct))
        {
            allPostsInDb.Add(post);
        }

        //Assert
        Assert.IsTrue(post1Result.IsSuccess);
        Assert.IsTrue(post2Result.IsSuccess);
        Assert.IsTrue(post3Result.IsSuccess);
        Assert.IsTrue(palpatinePostsInDb.Count() == 1);
        Assert.IsTrue(palpatinePostsInDb.Any(c => c.Title == post3Result.Value.Title));
        Assert.IsTrue(palpatinePostsInDb.Any(c => c.Content == post3Result.Value.Content));
        Assert.IsTrue(allPostsInDb.Count() == 3);
    }

    [TestMethod]
    public async Task GetAllPostsByAuthorId_ShouldReturnPosts()
    {
        //Arrange
        var darthLinuxerLoggedResult = await _userService.LoginAsync("darthlinuxer", "ChangeMe1$");
        var token = darthLinuxerLoggedResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var darthLinuxerId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var darthlinuxerName = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

        //Act
        //Adding 2 posts from Writer DarthLinuxer
        var post1Result = await _postService.AddAsync(new PostModelDTO(
            darthLinuxerId,
            "This is a Test Title",
            "This is a test Content"));

        var post2Result = await _postService.AddAsync(new PostModelDTO(
            darthLinuxerId,
            "This is another Title",
            "This is another Content"));

        var ct = new CancellationToken();
        var darthLinuxerPosts = _postService.GetAllByAuthorIdAsync(darthLinuxerId, ct, "Title", 1, 10, true, true, postStatus: Status.draft);
        List<PostModel> darthLinuxerPostsInDb = new();
        await foreach (var post in darthLinuxerPosts.WithCancellation(ct))
        {
            darthLinuxerPostsInDb.Add(post);
        }

        //Assert
        Assert.IsTrue(post1Result.IsSuccess);
        Assert.IsTrue(post2Result.IsSuccess);
        Assert.IsTrue(darthLinuxerPostsInDb.Count() == 2);
    }

    [TestMethod]
    public async Task GetAllPostsByTitle_ShouldReturnPosts()
    {
        //Arrange
        var darthLinuxerLoggedResult = await _userService.LoginAsync("darthlinuxer", "ChangeMe1$");
        var token = darthLinuxerLoggedResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var darthLinuxerId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var darthlinuxerName = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

        //Act
        //Adding 2 posts from Writer DarthLinuxer
        var post1Result = await _postService.AddAsync(new PostModelDTO(
             darthLinuxerId,
             "This is a Test Title",
             "This is a test Content"));


        var post2Result = await _postService.AddAsync(new PostModelDTO(
            darthLinuxerId,
            "This is another Title",
            "This is another Content"));

        var ct = new CancellationToken();
        var darthLinuxerPosts = _postService.GetAllByTitleAsync("another", ct, "Title", 1, 10, true, true, postStatus: Status.draft);
        List<PostModel> darthLinuxerPostsInDb = new();
        await foreach (var post in darthLinuxerPosts.WithCancellation(ct))
        {
            darthLinuxerPostsInDb.Add(post);
        }

        //Assert
        Assert.IsTrue(post1Result.IsSuccess);
        Assert.IsTrue(post2Result.IsSuccess);
        Assert.IsTrue(darthLinuxerPostsInDb.Count() == 1);
    }

    [TestMethod]
    public async Task GetAllPostsByContent_ShouldReturnPosts()
    {
        //Arrange
        var darthLinuxerLoggedResult = await _userService.LoginAsync("darthlinuxer", "ChangeMe1$");
        var token = darthLinuxerLoggedResult.Value;
        ClaimsPrincipal principal;
        TokenExtensions.ValidateJwtToken(token, out principal);
        var darthLinuxerId = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;
        var darthlinuxerName = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

        //Act
        //Adding 2 posts from Writer DarthLinuxer
        var post1Result = await _postService.AddAsync(new PostModelDTO(
               darthLinuxerId,
               "This is a Test Title",
               "This is a test Content"));


        var post2Result = await _postService.AddAsync(new PostModelDTO(
            darthLinuxerId,
            "This is another Title",
            "This is another Content"));

        var ct = new CancellationToken();
        var darthLinuxerPosts = _postService.GetAllByContentsAsync("another", ct, "Title", 1, 10, true, true, postStatus: Status.draft);
        List<PostModel> darthLinuxerPostsInDb = new();
        await foreach (var post in darthLinuxerPosts.WithCancellation(ct))
        {
            darthLinuxerPostsInDb.Add(post);
        }

        //Assert
        Assert.IsTrue(post1Result.IsSuccess);
        Assert.IsTrue(post2Result.IsSuccess);
        Assert.IsTrue(darthLinuxerPostsInDb.Count() == 1);
    }
}