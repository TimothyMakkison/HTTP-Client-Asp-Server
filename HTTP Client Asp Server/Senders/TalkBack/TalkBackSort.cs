using System;
using System.Linq;
using System.Net.Http;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackSort : BaseSender
    {
        public TalkBackSort(HttpClient client) : base(client)
        {
        }

        public void Process(string line)
        {
            if (!GetParameters(line, out string[] parameters))
                return;
            string uri = BuildQuery(parameters);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = SendAsync(request).Result;
            var product = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(product);
        }

        private bool GetParameters(string line, out string[] parameters)
        {
            // Takes read line of and remove command words
            var input = line.Replace("TalkBack Sort", "");
            var inputSpaceless = input.Replace(" ", "");

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

        private string BuildQuery(string[] parameters)
        {
            string uri = "talkback/sort";
            if (!(parameters.Length == 1 && parameters.First() == ""))
            {
                var namedValues = parameters.Select(x => $"integers={x}");
                uri += "?" + string.Join('&', namedValues);
            }
            return uri;
        }
    }
}