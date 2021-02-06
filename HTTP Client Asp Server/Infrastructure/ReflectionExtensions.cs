using HTTP_Client_Asp_Server.ConsoleClass;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public static class ReflectionExtensions
    {
        public static TargetType ToTargetType(this Type type)
        {
            return type == typeof(bool)
                       ? TargetType.Switch
                       : type == typeof(string)
                             ? TargetType.Scalar
                             : type.IsArray || typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(type)
                                   ? TargetType.Sequence
                                   : TargetType.Scalar;
        }

        public static bool IsPrimitiveEx(this Type type)
        {
            return
                   (type.GetTypeInfo().IsValueType && type != typeof(Guid))
                || type.GetTypeInfo().IsPrimitive
                || new[] {
                     typeof(string)
                    ,typeof(decimal)
                    ,typeof(DateTime)
                    ,typeof(DateTimeOffset)
                    ,typeof(TimeSpan)
                   }.Contains(type)
                || Convert.GetTypeCode(type) != TypeCode.Object;
        }
    }
}