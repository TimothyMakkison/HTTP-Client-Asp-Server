using HTTP_Client_Asp_Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackSort : BaseSender
    {
        public TalkBackSort(HttpClient client) : base(client)
        {
        }

        [Command("TalkBack Sort")]
        public string Process(IEnumerable<int> parameters)
        {
            string uri = BuildQuery(parameters);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = SendAsync(request).Result;
            var product = response.Content.ReadAsStringAsync().Result;

            return product;
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