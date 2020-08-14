using HTTP_Client_Asp_Server.Handlers;
using System;
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

        [Command("TalkBack Sort", Parsing = ParseMode.ParseAndTrim)]
        public async Task Process(string line)
        {
            if (!GetParameters(line, out string[] parameters))
                return;
            string uri = BuildQuery(parameters);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = SendAsync(request).Result;
            var product = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(product);
        }

        private bool GetParameters(string input, out string[] parameters)
        {
            //var inputSpaceless = input.Replace(" ", "");

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