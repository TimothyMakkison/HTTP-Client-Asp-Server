using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Net_Core_Server.Controllers.Tests;

public class TalkBackControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;

    public TalkBackControllerTests()
    {
        _factory = new WebApplicationFactory<Program>();
    }

    [Fact]
    public async Task TalkBackHelloReturnsOK()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/talkback/hello");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Hello World");
    }

    [Theory]
    [InlineData(new int[] { 1, 2, 4, 0 })]
    [InlineData(new int[] { 10 })]
    [InlineData(new int[] { 5, -5 })]
    public async Task SortArrayReturnOk(int[] integers)
    {
        // Arrange
        var sortedIntegers = integers.OrderBy(x => x);
        var client = _factory.CreateClient();
        var query = string.Join('&', integers.Select(i => $"integers={i}"));

        // Act
        var response = await client.GetAsync($"api/talkback/sort?{query}");

        // Assert
        var content = await response.Content.ReadAsStringAsync();
        var returnedIntegers = content.Trim('[').Trim(']').Split(",").Select(int.Parse).ToArray();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(Enumerable.SequenceEqual(returnedIntegers, sortedIntegers));
    }

    [Fact]
    public async Task SortArrayReturnBadRequestOnNull()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/talkback/sort");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}