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
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Xunit;

namespace Net_Core_Server.Controllers.Tests;

public class ProtectedEndpointTests
{
    const string BASE_ROUTE = "api/protected/";
    private readonly WebApplicationFactory<Program> _factory;
    private readonly FluentClient _client;

    private readonly User admin = new() { UserName="UserAdmin", Role = Role.Admin, ApiKey = Guid.NewGuid() };
    private readonly User user = new() { UserName="UserUser", Role = Role.User, ApiKey = Guid.NewGuid() };

    public ProtectedEndpointTests()
    {
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder
           .ConfigureServices(services =>
           {
               services.AddDbContext<UserContext>(opt => opt.UseInMemoryDatabase(nameof(ProtectedEndpointTests)));

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
    public async Task ProtectedHello_ShouldReturnHelloUsername_WhenLoggedIn()
    {
        // Act
        var response = await _client.GetAsync($"{BASE_ROUTE}hello").WithHeader("ApiKey", user.ApiKey.ToString());
        var body = await response.AsString();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.Contains($"Hello {user.UserName}", body);
    }

    [Fact]
    public async Task ProtectedSHA1_ShouldReturnHash_WhenLoggedIn()
    {
        // Act
        var response = await _client.GetAsync($"{BASE_ROUTE}sha1?message=hello").WithHeader("ApiKey", user.ApiKey.ToString());
        var body = await response.AsString();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.Contains("AAF4C61DDCC5E8A2DABEDE0F3B482CD9AEA9434D", body);
    }

    [Fact]
    public async Task ProtectedSHA256_ShouldReturnHash_WhenLoggedIn()
    {
        // Act
        var response = await _client.GetAsync($"{BASE_ROUTE}sha256?message=hello").WithHeader("ApiKey", user.ApiKey.ToString());
        var body = await response.AsString();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.Contains("2CF24DBA5FB0A30E26E83B2AC5B9E29E1B161E5C1FA7425E73043362938B9824", body);
    }

    [Fact]
    public async Task ProtectedGetPublicKey_ShouldKey_WhenLoggedIn()
    {
        // Act
        var response = await _client.GetAsync($"{BASE_ROUTE}getpublickey").WithHeader("ApiKey", user.ApiKey.ToString());
        var body = await response.AsString();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
        Assert.Contains("<RSAKeyValue><Modulus>", body);
    }
    [Fact]
    public async Task ProtectedGetSign_ShouldSignMessage_WhenLoggedIn()
    {
        // Act
        var response = await _client.GetAsync($"{BASE_ROUTE}sign").WithHeader("ApiKey", user.ApiKey.ToString()).WithArgument("message", "Hello");
        //var body = await response.AsString();

        //var keyReponse = await _client.GetAsync($"{BASE_ROUTE}getpublickey").WithHeader("ApiKey", user.ApiKey.ToString()).WithArgument("message", "Hello");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.Status);
    }

}