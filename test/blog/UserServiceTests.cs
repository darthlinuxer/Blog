namespace blog;

[TestClass]
public class UserServiceTests
{
    private IUserService _userService;
    private SharedSetup setup;

    public UserServiceTests()
    {
        setup = new SharedSetup();
        _userService = setup!.UserService;
    }

    [TestInitialize]
    public void Seed() => setup.SeedData();

    [TestMethod]
    public async Task GetUserById_ShouldReturnUser()
    {
        //Arrange

        //Act
        var result = await _userService!.RegisterAsync(new UserRecordDTO(
            username: "Anakin",
            email: "vader@deathstar.com",
            password: "123UpperLowerSymbol$",
            role: "Writer"));

        var addedUser = result.Value;

        var userInDb = await _userService.GetUserByIdAsync(addedUser.Id);

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(userInDb.Value.Id == addedUser.Id);
    }

    [TestMethod]
    public async Task GetUserByEmail_ShouldReturnUser()
    {
        //Arrange

        //Act
        var result = await _userService!.RegisterAsync(new UserRecordDTO(
            username: "Anakin",
            email: "vader@deathstar.com",
            password: "123UpperLowerSymbol$",
            role: "Writer"));
        var userInDb = await _userService.GetUserByEmailAsync("vader@deathstar.com");

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(userInDb.Value.Email == "vader@deathstar.com");
    }

    [TestMethod]
    public async Task GetUserByName_ShouldReturnUser()
    {
        //Arrange

        //Act
        var result = await _userService.RegisterAsync(new UserRecordDTO(
            username: "Anakin",
            email: "vader@deathstar.com",
            password: "123UpperLowerSymbol$",
            role: "Writer"));
        var userInDb = await _userService.GetUserByNameAsync("Anakin");

        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(userInDb.Value.UserName == "Anakin");
        Assert.IsTrue(userInDb.Value.Id is not null);
    }

    [TestMethod]
    public async Task CreateUserWithInvalidPassword_ShouldNotCreate()
    {
        //Arrange

        //Act
        var result = await _userService.RegisterAsync(new UserRecordDTO(
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

        //Act
        var result = await _userService.RegisterAsync(new UserRecordDTO(
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

        //Act
        var result1 = await _userService!.RegisterAsync(new UserRecordDTO(
           username: "Anakin",
           email: "vader@deathstar.com",
           password: "123UpperLowerSymbol$",
           role: "SithLord")); //will not add because SithLord is not a valid Role

        var result2 = await _userService!.RegisterAsync(new UserRecordDTO(
             username: "Leia",
             email: "leia@theprincess.com",
             password: "123UpperLowerSymbol$",
             role: "Editor"));

        var result3 = await _userService!.RegisterAsync(new UserRecordDTO(
           username: "Vader",
           email: "vader@deathstar.com",
           password: "123UpperLowerSymbol$",
           role: "Writer"));

        var result4 = await _userService!.RegisterAsync(new UserRecordDTO(
         username: "Palpatine",
         email: "emperor@deathstar.com",
         password: "123UpperLowerSymbol$",
         role: "Writer"));

        var writers = await _userService.GetAllUsersByRoleAsync("Writer");

        Assert.IsFalse(result1.IsSuccess);
        Assert.IsTrue(result2.IsSuccess);
        Assert.IsTrue(result3.IsSuccess);
        Assert.IsTrue(writers.Value.Count() == 3);
        Assert.IsNotNull(writers.Value.Any(c => c.UserName == "Vader"));
        Assert.IsNotNull(writers.Value.Any(c => c.UserName == "Palpatine"));
    }

    [TestMethod]
    public async Task LoginWithCorrectPassword_ShouldReturnValidToken()
    {
        //Arrange
        var result = await _userService!.RegisterAsync(
            new UserRecordDTO(
                            username: "Palpatine",
                            email: "emperor@deathstar.com",
                            password: "123UpperLowerSymbol$",
                            role: "Writer"));

        //Act

        var loginResult = await _userService.LoginAsync("Palpatine", "123UpperLowerSymbol$");
        var token = loginResult.Value;
        ClaimsPrincipal principal = null;
        var isTokenValid = TokenExtensions.ValidateJwtToken(token, out principal);
        var sid = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid);
        var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        var name = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        //Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsTrue(loginResult.IsSuccess);
        Assert.IsTrue(isTokenValid);
        Assert.IsTrue(principal.IsInRole("Writer"));
        Assert.IsTrue(name?.Value == "Palpatine");
        Assert.IsTrue(email?.Value == result.Value.Email);
        Assert.IsTrue(sid?.Value == result.Value.Id);
    }
}