using Client.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RailwaySharp;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;
using Assert = Xunit.Assert;

namespace Client.ConsoleClass.Tests
{
    [TestClass()]
    public class TypeConverterTests
    {
        [TestMethod()]
        public void ChangeTypeIEnumTest()
        {
            var values = Enumerable.Range(0, 10);
            var args = values.Select(x => x.ToString());
            var type = typeof(IEnumerable<int>);

            var output = TypeConverter.ChangeType(args, type, false, CultureInfo.InvariantCulture, false);

        }
        [Fact]
        public void TestTests()
        {
            Assert.True(true);
        }

        
    }
}