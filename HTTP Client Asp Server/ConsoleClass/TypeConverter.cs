using CSharpx;
using HTTP_Client_Asp_Server.Infrastructure;
using RailwaySharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

/// <summary>
/// From https://github.com/commandlineparser/commandline
/// https://github.com/commandlineparser/commandline/blob/master/src/CommandLine/Core/TypeConverter.cs
/// </summary>
namespace HTTP_Client_Asp_Server.ConsoleClass
{
    //public static class TypeConverter
    //{
    //    public static object ChangeType(IEnumerable<string> values, Type conversionType, bool scalar, CultureInfo conversionCulture)
    //    {
    //        return scalar
    //            ? ConvertString(values.SingleOrDefault(), conversionType, conversionCulture)
    //            : ChangeTypeSequence(values, conversionType, conversionCulture);
    //    }

    //    private static object ChangeTypeSequence(IEnumerable<string> values, Type conversionType, CultureInfo conversionCulture)
    //    {
    //        var type =
    //            conversionType.GetTypeInfo()
    //                          .GetGenericArguments()
    //                          .SingleOrDefault()
    //                          ?? throw new
    //                          InvalidOperationException("Non scalar properties should be sequence of type IEnumerable<T>.");

    //        var converted = values.Select(value => ConvertString(value, type, conversionCulture));

    //        //return converted.Any(a => a == null
    //        //    ? null
    //        //    : converted.Select(c => ((Just<object>)c).Value).ToUntypedArray(type);
    //        return converted;
    //    }
    //    private static object ConvertString(string value, Type type, CultureInfo conversionCulture)
    //    {
    //        try
    //        {
    //            return Convert.ChangeType(value, type, conversionCulture);
    //        }
    //        catch
    //        {
    //            //return default(type);
    //            // Required for converting from string to TimeSpan because Convert.ChangeType can't
    //            return System.ComponentModel.TypeDescriptor.GetConverter(type).ConvertFrom(null, conversionCulture, value);
    //        }
    //    }
    //}
    public static class TypeConverter
    {
        public static Maybe<object> ChangeType(IEnumerable<string> values, Type conversionType, bool scalar, CultureInfo conversionCulture, bool ignoreValueCase)
        {
            return scalar
                ? ChangeTypeScalar(values.Single(), conversionType, conversionCulture, ignoreValueCase)
                : ChangeTypeSequence(values, conversionType, conversionCulture, ignoreValueCase);
        }

        private static Maybe<object> ChangeTypeSequence(IEnumerable<string> values, Type conversionType, CultureInfo conversionCulture, bool ignoreValueCase)
        {
            var type =
                conversionType.GetTypeInfo()
                              .GetGenericArguments()
                              .SingleOrDefault()
                              .ToMaybe()
                              .FromJustOrFail(
                                  new InvalidOperationException("Non scalar properties should be sequence of type IEnumerable<T>.")
                    );

            var converted = values.Select(value => ChangeTypeScalar(value, type, conversionCulture, ignoreValueCase))
                                  .ToArray();

            var a= converted.Any(a => a.MatchNothing())
                ? Maybe.Nothing<object>()
                : Maybe.Just(converted.Select(c => ((Just<object>)c).Value).ToUntypedArray(type));
            return a;
        }

        private static Maybe<object> ChangeTypeScalar(string value, Type conversionType, CultureInfo conversionCulture, bool ignoreValueCase)
        {
            var result = ChangeTypeScalarImpl(value, conversionType, conversionCulture, ignoreValueCase);
            result.Match((_, __) => { }, e => e.First().RethrowWhenAbsentIn(
                 new[] { typeof(InvalidCastException), typeof(FormatException), typeof(OverflowException) }));
            return result.ToMaybe();
        }

        private static object ConvertString(string value, Type type, CultureInfo conversionCulture)
        {
            try
            {
                return Convert.ChangeType(value, type, conversionCulture);
            }
            catch (InvalidCastException)
            {
                // Required for converting from string to TimeSpan because Convert.ChangeType can't
                return System.ComponentModel.TypeDescriptor.GetConverter(type).ConvertFrom(null, conversionCulture, value);
            }
        }

        private static Result<object, Exception> ChangeTypeScalarImpl(string value, Type conversionType, CultureInfo conversionCulture, bool ignoreValueCase)
        {
            Func<object> changeType = () =>
            {
                Func<object> safeChangeType = () =>
                {
                    Func<Type> getUnderlyingType = () => Nullable.GetUnderlyingType(conversionType);

                    var type = getUnderlyingType() ?? conversionType;

                    Func<object> withValue =
                        () => ConvertString(value, type, conversionCulture);
                    Func<object> empty = () => null;

                    return (value == null) ? empty() : withValue();
                };

                return value.IsBooleanString() && conversionType == typeof(bool)
                    ? value.ToBoolean() : conversionType.GetTypeInfo().IsEnum
                        ? value.ToEnum(conversionType, ignoreValueCase) : safeChangeType();
            };

            Func<object> makeType = () =>
            {
                try
                {
                    var ctor = conversionType.GetTypeInfo().GetConstructor(new[] { typeof(string) });
                    return ctor.Invoke(new object[] { value });
                }
                catch (Exception)
                {
                    throw new FormatException("Destination conversion type must have a constructor that accepts a string.");
                }
            };
            //if (conversionType.IsCustomStruct()) return Result.Try(makeType);
            return (conversionType.IsPrimitiveEx()
                    ? changeType
                    : makeType).ResultTry();
        }

        private static object ToEnum(this string value, Type conversionType, bool ignoreValueCase)
        {
            object parsedValue;
            try
            {
                parsedValue = Enum.Parse(conversionType, value, ignoreValueCase);
            }
            catch (ArgumentException)
            {
                throw new FormatException();
            }
            if (IsDefinedEx(parsedValue))
            {
                return parsedValue;
            }
            throw new FormatException();
        }

        private static bool IsDefinedEx(object enumValue)
        {
            char firstChar = enumValue.ToString()[0];
            if (Char.IsDigit(firstChar) || firstChar == '-')
                return false;

            return true;
        }
    }
}