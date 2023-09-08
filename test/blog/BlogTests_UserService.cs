using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace blog;

public partial class BlogTest
{

    [TestMethod]
    public async Task GetUserByName_ShouldReturnUser()
    {
        //Arrange
        var userManager = _serviceProvider.GetService<UserManager<BlogUser>>();

        //Act
        var result = await userManager!.CreateAsync(new BlogUser { 
            UserName = "Camilo",
            Email = "chaves.camilo@gmail.com", 
            PasswordHash = "thisIsALongPasswordHash" }, password: "UpperLower1$");
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
        var result1 = await userManager!.CreateAsync(new BlogUser { 
            UserName = "DarthLinuxer",
            Email = "DarthLinuxer@gmail.com",
            PasswordHash = "thisIsALongPasswordHash" }, password: "UpperLower1$");

        var result2 = await userManager!.CreateAsync(new BlogUser { 
            UserName = "Anakin",
            Email = "vader@deathstar.com",
            PasswordHash = "thisIsALongPasswordHash" }, password: "TheEmperorSucks69$");

        var result3 = await userManager!.CreateAsync(new BlogUser { 
            UserName = "Leia", 
            Email = "princess@galaxy.com",
            PasswordHash = "thisIsALongPasswordHash" }, password: "HanSoloForeverLove2$");

        var camilo = await userManager.FindByNameAsync("DarthLinuxer");
        await userManager.AddClaimAsync(camilo, new Claim(ClaimTypes.Role, "Public"));

        var anakin = await userManager.FindByNameAsync("Anakin");
        await userManager.AddClaimAsync(anakin, new Claim(ClaimTypes.Role, "Writer"));

        var leia = await userManager.FindByNameAsync("Leia");
        await userManager.AddClaimAsync(leia, new Claim(ClaimTypes.Role, "Writer"));

        var writers = await userManager.GetUsersForClaimAsync(new Claim(ClaimTypes.Role, "Writer"));
        
        Assert.IsTrue(result1.Succeeded);
        Assert.IsTrue(result2.Succeeded);
        Assert.IsTrue(result3.Succeeded);
        Assert.IsTrue(writers.Count == 2);
    }

}