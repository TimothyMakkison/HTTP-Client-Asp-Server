using HTTP_Client_Asp_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackSort : BaseSender
    {
        public TalkBackSort(HttpClient client) : base(client)
        {
        }

        [Command("TalkBack Sort")]
        public void Process(IEnumerable<int> parameters)
        {
            string uri = BuildQuery(parameters);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = SendAsync(request).Result;
            var product = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(product);
        }

        private static bool GetParameters(string input, out string[] parameters)
        {
            var inputSpaceless = input;
            // Check string is in correct form, print error if incorrect
            if (!(inputSpaceless.Contains('[') && inputSpaceless.Contains(']')))
            {
                parameters = null;
                Console.WriteLine("Invalid array form.");
                return false;
            }
            var parameterString = inputSpaceless.Replace("[", string.Empty).Replace("]", string.Empty);
            parameters = parameterString.Split(',');
            return true;
        }

        private static string BuildQuery(IEnumerable<int> parameters)
        {
            string uri = "talkback/sort";
            var namedValues = parameters.Select(x => $"integers={x}");
            uri += "?" + string.Join('&', namedValues);
            return uri;
        }
    }
}