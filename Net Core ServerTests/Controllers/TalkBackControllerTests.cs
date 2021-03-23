using Net_Core_Server.Controllers;
using System;
using System.Collections.Generic;
using Xunit;
using System.Text;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;

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

            Assert.True(result.Result is OkObjectResult);
        }

        [Theory]
        [InlineData(new int[] { 1,2,4,0})]
        [InlineData(new int[] { 10})]
        [InlineData(new int[] { 10})]
        [InlineData(new int[] { 5,-5 })]

        public void SortArrayReturnOk(IEnumerable<int> integers)
        {
            var result = _talkBackController.GetSort(integers.ToList());

            Assert.True(result.Result is OkObjectResult);
        }

        [Fact]
        public void SortArrayReturnBadRequestOnNull()
        {
            var result = _talkBackController.GetSort(null);

            Assert.True(result.Result is BadRequestObjectResult);
        }
    }
}