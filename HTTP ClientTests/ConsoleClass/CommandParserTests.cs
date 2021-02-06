using HTTP_Client_Asp_Server.ConsoleClass;
using HTTP_Client_Asp_Server.Models.CommandModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace HTTP_Client_Asp_Server.ConsoleClass.Tests
{
    [TestClass()]
    public class CommandParserTests
    {
        private void Method (IEnumerable<int> collection)
        {

        }
        [TestMethod()]
        public void ParseTest1()
        {
            var cd = new CommandData() { CommandKey = "Command" };
            Assert.Fail();
        }
    }
}