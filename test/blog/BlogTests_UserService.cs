using webapi.Extensions;

namespace blog;

public partial class BlogTest
{
    [TestMethod]
    public async Task GetUserById_ShouldReturnUser()
    {
        //Arrange
        var userService = _serviceProvider.GetService<IUserService>();

        //Act
        var result = await userService!.RegisterAsync(new UserRecord(
            username: "Anakin",
            email: "vader@deathstar.com",
            password: "123UpperLowerSymbol$",
            role: "Writer"));

        var addedUser = result.Value;

        var userInDb = await userService.GetUserByIdAsync(addedUser.Id);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(userInDb.Value.Id == addedUser.Id);
    }

    [TestMethod]
    public async Task GetUserByEmail_ShouldReturnUser()
    {
        //Arrange
        var userService = _serviceProvider.GetService<IUserService>();

        //Act
        var result = await userService!.RegisterAsync(new UserRecord(
            username: "Anakin",
            email: "vader@deathstar.com",
            password: "123UpperLowerSymbol$",
            role: "Writer"));
        var userInDb = await userService.GetUserByEmailAsync("vader@deathstar.com");

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(userInDb.Value.Email == "vader@deathstar.com");
    }

    [TestMethod]
    public async Task GetUserByName_ShouldReturnUser()
    {
        //Arrange
        var userService = _serviceProvider.GetService<IUserService>();

        //Act
        var result = await userService!.RegisterAsync(new UserRecord(
            username: "Anakin",
            email: "vader@deathstar.com",
            password: "123UpperLowerSymbol$",
            role: "Writer"));
        var userInDb = await userService.GetUserByNameAsync("Anakin");

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(userInDb.Value.UserName == "Anakin");
        Assert.IsTrue(userInDb.Value.Id is not null);
    }

    [TestMethod]
    public async Task CreateUserWithInvalidPassword_ShouldNotCreate()
    {
        //Arrange
        var userService = _serviceProvider.GetService<IUserService>();

        //Act
        var result = await userService!.RegisterAsync(new UserRecord(
            username: "Anakin",
            email: "vader@deathstar.com",
            password: "123",
            role: "Writer"));

        //Assert
        Assert.IsFalse(result.IsSuccess);
    }

    [TestMethod]
    public async Task CreateUserInInexistentRole_ShouldNotCreate()
    {
        //Arrange
        var userService = _serviceProvider.GetService<IUserService>();

        //Act
        var result = await userService!.RegisterAsync(new UserRecord(
            username: "Anakin",
            email: "vader@deathstar.com",
            password: "123UpperLowerSymbol$",
            role: "Emperor"));

        //Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.IsTrue(string.Compare(result.Errors.First(), "Role Emperor is invalid!", true) == 0);
    }

    [TestMethod]
    public async Task GetAllUsersByRole_ShouldReturnAllUsersInRole()
    {
        //Arrange
        var service = _serviceProvider.GetService<IUserService>();

        //Act
        var result1 = await service!.RegisterAsync(new UserRecord(
           username: "Anakin",
           email: "vader@deathstar.com",
           password: "123UpperLowerSymbol$",
           role: "SithLord")); //will not add because SithLord is not a valid Role

        var result2 = await service!.RegisterAsync(new UserRecord(
             username: "Leia",
             email: "leia@theprincess.com",
             password: "123UpperLowerSymbol$",
             role: "Editor"));

        var result3 = await service!.RegisterAsync(new UserRecord(
           username: "Luke",
           email: "Luke@Rebellion.com",
           password: "123UpperLowerSymbol$",
           role: "Public"));

        var result4 = await service!.RegisterAsync(new UserRecord(
           username: "Vader",
           email: "vader@deathstar.com",
           password: "123UpperLowerSymbol$",
           role: "Writer"));

        var result5 = await service!.RegisterAsync(new UserRecord(
         username: "Palpatine",
         email: "emperor@deathstar.com",
         password: "123UpperLowerSymbol$",
         role: "Writer"));

        var writers = await service.GetAllUsersByRoleAsync("Writer");

        Assert.IsFalse(result1.IsSuccess);
        Assert.IsTrue(result2.IsSuccess);
        Assert.IsTrue(result3.IsSuccess);
        Assert.IsTrue(result4.IsSuccess);
        Assert.IsTrue(writers.Value.Count() == 2);
        Assert.IsNotNull(writers.Value.Any(c => c.UserName == "Vader"));
        Assert.IsNotNull(writers.Value.Any(c => c.UserName == "Palpatine"));
    }

    [TestMethod]
    public async Task LoginWithCorrectPassword_ShouldReturnValidToken()
    {
        //Arrange
        var service = _serviceProvider.GetService<IUserService>();
        var result = await service!.RegisterAsync(
            new UserRecord(
                            username: "Palpatine",
                            email: "emperor@deathstar.com",
                            password: "123UpperLowerSymbol$",
                            role: "Writer"));

        //Act

        var loginResult = await service.LoginAsync("Palpatine", "123UpperLowerSymbol$");
        var token = loginResult.Value;
        ClaimsPrincipal principal = null;
        var isTokenValid = TokenExtensions.ValidateJwtToken(token, out principal);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(loginResult.IsSuccess);
        Assert.IsTrue(isTokenValid);
        Assert.IsTrue(principal.IsInRole("Writer"));
        Assert.IsTrue(principal.Identity.Name == "Palpatine");
    }
}