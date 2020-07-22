using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackSender : BaseSender
    {
        public TalkBackSender(HttpClient client) : base(client) { }

        public async void HelloWorld(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "talkback/hello");
            HttpResponseMessage response = await SendAsync(request);
            var product = await GetResponseString(response);
            Console.WriteLine(product);
        }
        public void Sort(string line)
        {
            string uri = "talkback/sort" + ExtractParameters(line);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = SendAsync(request).Result;
            var product = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(product);
        }
        private string ExtractParameters(string line)
        {
            // Takes read line of TalkBack Sort [1,2,3] and generates query parameters.
            var input = line.Replace("TalkBack Sort", "");
            var inputSpaceless = input.Replace(" ", "");

            var values = inputSpaceless.Replace("[", string.Empty);
            values = values.Replace("]", string.Empty);
            var valueArray = values.Split(',');

            if (!(values.Length == 1 && valueArray.First() == ""))
            {
                var namedValues = values.Select(x => $"integers={x}");
                return "?" + string.Join('&', namedValues);
            }
            return "";
        }
    }
}
