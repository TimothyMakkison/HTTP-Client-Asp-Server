using CSharpx;
using HTTP_Client_Asp_Server.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HTTP_Client_Asp_Server.ConsoleClass.Tests
{
    [TestClass()]
    public class TypeConverterTests
    {
        [TestMethod()]
        public void ChangeTypeScalarTest()
        {
            var value = 53;
            var args = value.ToString().Yield();

            var type = typeof(int);

            var output = TypeConverter.ChangeType(args, type, true, CultureInfo.InvariantCulture, false);
            output.Match(val => Assert.AreEqual(value, val), () => Assert.Fail());
        }

        [TestMethod()]
        public void ChangeTypeIEnumTest()
        {
            var values = Enumerable.Range(0, 10);
            var args = values.Select(x => x.ToString());
            var type = typeof(IEnumerable<int>);

            var output = TypeConverter.ChangeType(args, type, false, CultureInfo.InvariantCulture, false);

            output.Match(val => Enumerable.SequenceEqual((IEnumerable<int>)val, values), () => Assert.Fail());
        }
    }
}