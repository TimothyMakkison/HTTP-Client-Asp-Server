using System;
using System.Linq;
using System.Net.Http;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackSender : BaseSender
    {
        public TalkBackSender(HttpClient client) : base(client)
        {
        }

        public async void HelloWorld(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "talkback/hello");
            HttpResponseMessage response = await SendAsync(request);
            var product = await GetResponseString(response);
            Console.WriteLine(product);
        }

        public void Sort(string line)
        {
            // Takes read line of and remove command words
            var input = line.Replace("TalkBack Sort", "");
            var inputSpaceless = input.Replace(" ", "");

            // Check string is in correct form, print error if incorrect
            if (!(inputSpaceless.Contains('[') && inputSpaceless.Contains(']')))
            {
                Console.WriteLine("Invalid array form.");
                return;
            }
            var parameterString = inputSpaceless.Replace("[", string.Empty).Replace("]", string.Empty);
            var values = parameterString.Split(',');

            // Add parameters to string.
            string uri = "talkback/sort";
            if (!(values.Length == 1 && values.First() == ""))
            {
                var namedValues = values.Select(x => $"integers={x}");
                uri += "?" + string.Join('&', namedValues);
            }

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = SendAsync(request).Result;
            var product = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(product);
        }
    }
}