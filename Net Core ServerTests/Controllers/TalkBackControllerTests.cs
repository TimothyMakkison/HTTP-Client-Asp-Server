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

            Assert.True(result.Result is OkObjectResult);
            Assert.Equal("Hello World", responseString);
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

            Assert.True(response.Result is OkObjectResult);
            Assert.Equal(returnedIntegers, sortedIntegers);
        }

        [Fact]
        public void SortArrayReturnBadRequestOnNull()
        {
            var result = _talkBackController.GetSort(null);

            Assert.True(result.Result is BadRequestObjectResult);
        }
    }
}