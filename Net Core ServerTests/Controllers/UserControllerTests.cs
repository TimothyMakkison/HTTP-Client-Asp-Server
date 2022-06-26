using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Net_Core_Server.Models;
using Net_Core_ServerTests.Data;
using Net_Core_ServerTests.Infrastructure;
using System.Collections.Generic;
using Xunit;

namespace Net_Core_Server.Controllers.Tests;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly TestUserDataAccess _userData;

    public UserControllerTests()
    {
        _userData = new TestUserDataAccess();
        _controller = new UserController(_userData);
    }

    [Fact]
    public async void UserControllerContainsUser_Test()
    {
        var name = "Jack";
        var user = new User()
        {
            UserName = name,
        };
        _userData.SetDataContext(new List<User> { user });

        var response = await _controller.GetUser(name);
        var ok = response.Result as OkObjectResult;
        var returnedString = ok.Value as string;

        returnedString.Should().StartWith("True");
    }

    [Fact]
    public async void UserControllerDoesNotContainUser_Test()
    {
        var name = "Jack";
        var user = new User()
        {
            UserName = name,
        };
        _userData.SetDataContext(new List<User> { user });

        var response = await _controller.GetUser("Sophie");
        var returnedString = ResultExtensions.OKResponseToType(response);

        returnedString.Should().StartWith("False");
    }

    [Fact]
    public async void UserControllerPostNewUser_Test()
    {
        _userData.SetDataContext(new List<User>());

        var response = await _controller.PostNewUser("Sophie");

        response.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async void UserControllerReturnsErrorIfUserExists_Test()
    {
        var name = "Jack";
        var user = new User()
        {
            UserName = name,
        };
        _userData.SetDataContext(new List<User> { user });

        var response = await _controller.PostNewUser(name);

        response.Result.Should().BeOfType<ForbidResult>();
    }

    [Fact]
    public async void UserControllerReturnsBadRequestIfNull_Test()
    {
        _userData.SetDataContext(new List<User>());

        var response = await _controller.PostNewUser(null);

        response.Result.Should().BeOfType<BadRequestObjectResult>();
    }
}
