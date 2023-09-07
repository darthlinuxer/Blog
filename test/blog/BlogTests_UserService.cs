namespace blog;

public partial class BlogTest
{

    [TestMethod]
    public async Task GetUserById_ShouldReturnUser()
    {
        //Arrange
        var UserService = _serviceProvider.GetService<IUserService>();

        //Act
        var addedUser = await UserService!.AddAsync(new User {Username="Camilo",Password="123",Role=UserRole.Writer });
        await UserService.CompleteAsync();
        var userInDb = await UserService.GetAsync(p=>p.UserId == addedUser!.UserId, CancellationToken.None, true, null);

        //Assert
        Assert.IsNotNull(addedUser);
        Assert.AreEqual(addedUser.UserId, userInDb!.UserId);
        Assert.AreEqual(addedUser.Username, userInDb.Username);
    }

     public async Task GetAllUsersByRole_ShouldReturnAllUsers()
    {
        //Arrange
        var UserService = _serviceProvider.GetService<IUserService>();

        //Act
        var camilo = await UserService!.AddAsync(new User {Username="Camilo",Password="123",Role=UserRole.Writer });
        var anakin = await UserService!.AddAsync(new User {Username="Anakin",Password="123",Role=UserRole.Public });
        var leia = await UserService!.AddAsync(new User {Username="Leia",Password="123",Role=UserRole.Public });
        await UserService.CompleteAsync();
        var allPublicUsers = UserService.GetAllUsersByRole(role: UserRole.Public,
                                                     page: 1,
                                                     count: 10,
                                                     descending: true,
                                                     asNoTracking: true,
                                                     navigation: null,
                                                     CancellationToken.None);
        
        //Assert
        Assert.IsNotNull(addedUser);
        Assert.AreEqual(addedUser.UserId, userInDb!.UserId);
        Assert.AreEqual(addedUser.Username, userInDb.Username);
    }

    [TestMethod]
    public async Task GetNotExistentUserById_ShouldReturnNull()
    {
        //Arrange
        var UserService = _serviceProvider.GetService<IUserService>();

        //Act
        var userInDb = await UserService!.GetAsync(p=>p.UserId==10, CancellationToken.None, true, null);

        //Assert
        Assert.IsNull(userInDb);
    }
    
}