using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Net_Core_Server.Data;
using Net_Core_Server.Models;
using Pathoschild.Http.Client;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Net_Core_Server.Controllers.Tests;

public class UserControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly FluentClient _client;

    private readonly User admin = new() { UserName="UserAdmin", Role = Role.Admin, ApiKey = Guid.NewGuid() };
    private readonly User user = new() { UserName="UserUser", Role = Role.User, ApiKey = Guid.NewGuid() };

    public UserControllerTests()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder
           .ConfigureServices(services =>
           {
               services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase("UserListTesting"));

               var sp = services.BuildServiceProvider();
               using var scope = sp.CreateScope();
               var serviceProvider = scope.ServiceProvider;
               var _userContext = serviceProvider.GetRequiredService<UserContext>();
               _userContext.Database.EnsureCreated();

               _userContext.Users.Add(admin);
               _userContext.Users.Add(user);
               _userContext.SaveChanges();
           }));

        var client = _factory.CreateClient();

        var fluentClient = new FluentClient(null, client);
        fluentClient.Filters.Clear();
        _client = fluentClient;
    }

    [Fact]
    public async Task UserControllerContainsUser_Test()
    {
        // Act
        var response = await _client.GetAsync($"api/user/new?username={user.UserName}");
        var body = await response.AsString();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.Contains("True - User Does Exist!", body);
    }

    [Fact]
    public async void UserControllerDoesNotContainUser_Test()
    {
        // Arange
        var username = "IDon'tExist";

        // Act
        var response = await _client.GetAsync($"api/user/new?username={username}");
        var body = await response.AsString();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.Contains("False - User Does Not Exist! Did you mean to do a POST to create a new user?", body);
    }

    [Fact]
    public async void UserControllerPostNewUser_Test()
    {
        // Arange
        var newUsername = "NewUser";

        // Act
        var response = await _client.PostAsync($"api/user/new?username={newUsername}");
        var newUser = await response.As<User>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.Equal(newUsername, newUser.UserName);
    }

    [Fact]
    public async void UserControllerReturnsErrorIfUserExists_Test()
    {
        // Act
        var response = await _client.PostAsync($"api/user/new?username={user.UserName}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.Status);
    }

    [Fact]
    public async void UserControllerReturnsBadRequestIfNull_Test()
    {
        // Act
        var response = await _client.PostAsync($"api/user/new?username={user.UserName}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.Status);
    }

    [Fact]
    public async void UserControllerReturnsOk_WhenDeletingSelf()
    {
        // Act
        var response = await _client.DeleteAsync($"api/user/removeuser?username={user.UserName}")
            .WithHeader("ApiKey", user.ApiKey.ToString());
        var success = await response.As<bool>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.True(success);
    }

    [Fact]
    public async void UserController_ShouldReturnOkFalse_WhenDeletingFakeUser()
    {
        // Act
        var fakeUsername = "Doesnt'tExist";
        var response = await _client.DeleteAsync($"api/user/removeuser?username={fakeUsername}").WithHeader("ApiKey", user.ApiKey.ToString());
        var success = await response.As<bool>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.False(success);
    }

    [Fact]
    public async void UserController_ShouldReturnUnauthorized_WhenInvalidHeader()
    {
        // Act
        var fakeUsername = "Doesnt'tExist";
        var response = await _client.DeleteAsync($"api/user/removeuser?username={fakeUsername}").WithHeader("ApiKey", Guid.NewGuid().ToString());

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.Status);
    }

    [Fact]
    public async void UserController_ShouldReturnOk_WhenAdmin()
    {
        // Arange
        var roleRequest = new ChangeRoleRequest() { Username =user.UserName, Role = Role.Admin };

        // Act
        var response = await _client.PostAsync($"api/user/changerole")
                                    .WithHeader("ApiKey", admin.ApiKey.ToString())
                                    .WithBody(roleRequest);
        var body = await response.As<string>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.Contains(body, "DONE");
    }

    [Fact]
    public async void UserController_ShouldReturnBadRequest_WhenRoleDoesNotExist()
    {
        // Arange
        var roleRequest = new ChangeRoleRequest() { Username =user.UserName, Role = "FakeRole" };

        // Act
        var response = await _client.PostAsync($"api/user/changerole")
                                    .WithHeader("ApiKey", admin.ApiKey.ToString())
                                    .WithBody(roleRequest);
        var body = await response.As<string>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.Status);
        Assert.Contains(body, "NOT DONE: Role does not exist");
    }

    [Fact]
    public async void UserController_ShouldReturnBadRequest_WhenUserDoesNotExist()
    {
        // Arange
        var roleRequest = new ChangeRoleRequest() { Username ="FakeUser", Role = Role.Admin };

        // Act
        var response = await _client.PostAsync($"api/user/changerole")
                                    .WithHeader("ApiKey", admin.ApiKey.ToString())
                                    .WithBody(roleRequest);
        var body = await response.As<string>();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.Status);
        Assert.Contains(body, "NOT DONE: Username does not exist");
    }
}