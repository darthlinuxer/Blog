using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace blog;

public partial class BlogTest
{

    [TestMethod]
    public async Task GetUserById_ShouldReturnUser()
    {
        //Arrange
        var userManager = _serviceProvider.GetService<UserManager<BlogUser>>();

        //Act
        var result = await userManager.CreateAsync(new BlogUser { UserName = "Camilo", PasswordHash = "abc" }, password: "123");
        var userInDb = await userManager.FindByNameAsync("Camilo");

        //Assert
        Assert.IsTrue(result.Succeeded);
        Assert.IsTrue(userInDb.UserName == "Camilo");
    }

    [TestMethod]
    public async Task GetAllUsersByRole_ShouldReturnAllUsers()
    {
        //Arrange
        var userManager = _serviceProvider.GetService<UserManager<BlogUser>>();

        //Act
        var result1 = await userManager!.CreateAsync(new BlogUser { UserName = "Camilo", PasswordHash = "abc" }, password: "123");
        var result2 = await userManager!.CreateAsync(new BlogUser { UserName = "Anakin", PasswordHash = "abc" }, password: "123");
        var result3 = await userManager!.CreateAsync(new BlogUser { UserName = "Leia", PasswordHash = "123" }, password: "123");

        var camilo = await userManager.FindByNameAsync("Camilo");
        await userManager.AddClaimAsync(camilo, new Claim(ClaimTypes.Role, "Public"));

        var anakin = await userManager.FindByNameAsync("Anakin");
        await userManager.AddClaimAsync(anakin, new Claim(ClaimTypes.Role, "Writer"));

        var leia = await userManager.FindByNameAsync("Leia");
        await userManager.AddClaimAsync(leia, new Claim(ClaimTypes.Role, "Writer"));

        var writers = await userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Writer"));
        Assert.IsTrue(writers.Count == 2);
    }

}