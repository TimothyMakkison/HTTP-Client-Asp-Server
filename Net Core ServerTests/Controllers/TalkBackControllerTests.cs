using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Net_Core_ServerTests.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Net_Core_Server.Controllers.Tests
{
    public class TalkBackControllerTests
    {
        private readonly TalkBackController _talkBackController;

        public TalkBackControllerTests()
        {
            _talkBackController = new TalkBackController(); ;
        }

        [Fact]
        public void TalkBackHelloReturnsOK()
        {
            var result = _talkBackController.GetHello();
            var responseString = ResultExtensions.OKResponseToType(result);

            result.Result.Should().BeOfType<OkObjectResult>();
            responseString.Should().BeEquivalentTo("Hello World");
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 4, 0 })]
        [InlineData(new int[] { 10 })]
        [InlineData(new int[] { 5, -5 })]
        [InlineData(new int[] { })]

        public void SortArrayReturnOk(IEnumerable<int> integers)
        {
            var sortedIntegers = integers.OrderBy(x => x);

            var response = _talkBackController.GetSort(integers.ToList());
            var ok = response.Result as OkObjectResult;
            var returnedIntegers = ok.Value as IEnumerable<int>;

            response.Result.Should().BeOfType<OkObjectResult>();
            returnedIntegers.Should().BeEquivalentTo(sortedIntegers);
        }

        [Fact]
        public void SortArrayReturnBadRequestOnNull()
        {
            var result = _talkBackController.GetSort(null);

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}