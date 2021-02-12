using System;
using System.Linq;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public static class StringExtensions
    {
        public static string[] ToArgs(this string input, string commandKey)
        {
            return input.Replace(commandKey, "")
                            .Trim()
                            .Split()
                            .Where(arg => arg != default && arg != "")
                            .ToArray();
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