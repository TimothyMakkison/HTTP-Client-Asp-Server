using HTTP_Client_Asp_Server.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HTTP_Client_Asp_Server.ConsoleClass.Tests
{
    [TestClass()]
    public class CommandParserTests
    {
        [TestMethod()]
        public void ParseTest1()
        {
            var cd = new CommandData() { CommandKey = "Command" };
            Assert.Fail();
        }
    }
}