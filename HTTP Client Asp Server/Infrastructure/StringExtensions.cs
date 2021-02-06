using System;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public static class StringExtensions
    {
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