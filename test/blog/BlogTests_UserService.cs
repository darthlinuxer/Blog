namespace blog;

public partial class BlogTest
{

    [TestMethod]
    public async Task GetUserById_ShouldReturnUser()
    {
        //Arrange
        var UserService = _serviceProvider.GetService<IUserService>();

        //Act
        var addedUser = await UserService!.AddAsync(new User { UserName = "Camilo", PasswordHash = "123", Role = UserRole.Writer });
        await UserService.CompleteAsync();
        var userInDb = await UserService.GetAsync(p => p.Id == addedUser!.Id, CancellationToken.None, true, null);

        //Assert
        Assert.IsNotNull(addedUser);
        Assert.AreEqual(addedUser.Id, userInDb!.Id);
        Assert.AreEqual(addedUser.UserName, userInDb.UserName);
    }

    [TestMethod]
    public async Task GetAllUsersByRole_ShouldReturnAllUsers()
    {
        //Arrange
        var UserService = _serviceProvider.GetService<IUserService>();

        //Act
        var camilo = await UserService!.AddAsync(new User { UserName = "Camilo", PasswordHash = "123", Role = UserRole.Writer });
        var anakin = await UserService!.AddAsync(new User { UserName = "Anakin", PasswordHash = "123", Role = UserRole.Public });
        var leia = await UserService!.AddAsync(new User { UserName = "Leia", PasswordHash = "123", Role = UserRole.Public });
        await UserService.CompleteAsync();
        var allPublicUsers = UserService.GetAllUsersByRole(role: UserRole.Public,
                                                     page: 1,
                                                     count: 10,
                                                     descending: true,
                                                     asNoTracking: true,
                                                     navigation: null,
                                                     CancellationToken.None);
        int publicCount = 0;
        await foreach (var publicUser in allPublicUsers)
        {
            if (publicUser.Role == UserRole.Public) publicCount++;
        }

        //Assert
        Assert.IsTrue(publicCount == 2);
    }

    [TestMethod]
    public async Task GetNotExistentUserById_ShouldReturnNull()
    {
        //Arrange
        var UserService = _serviceProvider.GetService<IUserService>();

        //Act
        var userInDb = await UserService!.GetAsync(p => p.Id == "10", CancellationToken.None, true, null);

        //Assert
        Assert.IsNull(userInDb);
    }

}