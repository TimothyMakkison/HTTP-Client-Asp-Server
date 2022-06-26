using Client.ConsoleClass;
using Client.Infrastructure;
using RailwaySharp;
using System.Collections.Generic;
using System.Globalization;
using Xunit;

namespace Client_Tests;

public class TypeConverterTests
{
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

    [Theory]
    [InlineData(new string[] { "1", "2", "3" }, new int[]{ 1, 2, 3 })]
    public void TypeConverterCanConvertToCollectionTheory(IEnumerable<string> stringForm, IEnumerable<int> expected)
    {
        var type = typeof(IEnumerable<int>);

        var output = TypeConverter.ChangeType(stringForm, type, false, CultureInfo.InvariantCulture, true);
        object value = output.SucceededWith();

        Assert.Equal((IEnumerable<int>)value, expected);
    }
}
