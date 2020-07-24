using System.Collections.Generic;
using System.Linq;

namespace HTTP_Client_Asp_Server.Handlers
{
    public static class UriBuilder
    {
        public static string Build(string baseUri, IEnumerable<Pair> valuePairs)
        {
            if (valuePairs.Count() == 0)
            {
                return baseUri;
            }
            baseUri += "?";
            var pairings = valuePairs.Select(x => $"{x.Name}={x.Value}");
            var parameters = string.Join('&', pairings);
            return baseUri += parameters;
        }
    }

    public struct Pair
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}