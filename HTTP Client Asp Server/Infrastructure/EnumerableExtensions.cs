﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Yield<T>(this T value)
        {
            yield return value;
        }

        public static object ToUntypedArray(this IEnumerable<object> value, Type type)
        {
            var array = Array.CreateInstance(type, value.Count());
            value.ToArray().CopyTo(array, 0);
            return array;
        }
    }
}