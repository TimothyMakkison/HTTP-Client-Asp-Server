using System;
using System.Collections.Generic;
using System.Linq;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public static class StringExtensions
    {
        public static IEnumerable<string> RemoveValueAndSplit(this string input, string removedWord)
        {
            return input.Replace(removedWord, "")
                        .Trim()
                        .Split()
                        .Where(arg => arg != default && arg != "");
        }

        public static bool IsBooleanString(this string value)
        {
            return value.Equals("true", StringComparison.OrdinalIgnoreCase)
                || value.Equals("false", StringComparison.OrdinalIgnoreCase);
        }

        public static bool ToBoolean(this string value)
        {
            return value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }
    }
}