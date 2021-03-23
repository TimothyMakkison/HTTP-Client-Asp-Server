using System;
using System.Globalization;
using Xunit;

namespace Client_Tests
{
    public class ConsoleClassTests
    {
        [Fact]
        public void Test1()
        {

        }

        [Theory]
        [InlineData("hello", "hello")]
        [InlineData("10", 10)]
        [InlineData("1.5", 1.5)]
        [InlineData("c", 'c')]
        public void TypeConverterCanConvertScalarValuesTheory<T>(string stringForm, T expected)
        {
            var type = typeof(T);
            var output = TypeConverter.ChangeType(stringForm.Yield(), type, true, CultureInfo.InvariantCulture, false);

            object value = output.SucceededWith();
            Assert.Equal((T)value, expected);
        }
    }
}
