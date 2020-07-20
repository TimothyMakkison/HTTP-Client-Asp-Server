using HTTP_Client_Asp_Server.Handlers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackSender : BaseSender
    {
        public TalkBackSender(HttpClient client) : base(client) { }

        public void HelloWorld(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "talkback/hello");
            HttpResponseMessage response = SendAsync(request).Result;
            var product = GetResponseString(response).Result;
            Console.WriteLine(product);
        }
        public void Sort(string line)
        {
            line = line.Replace("TalkBack Sort", "");
            line = line.Replace(" ", "");

            var values = Split(line);
            var valuePairs = values.Select(x => new Pair() { Name = "integer", Value = x });

            var uri = Handlers.UriBuilder.Build("talkback/sort", valuePairs);

            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            HttpResponseMessage response = SendAsync(request).Result;
            var product = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(product);
        }
        private IEnumerable<string> Split(string value)
        {
            value = value.Replace("[", "");
            value = value.Replace("]", "");

            return value.Split(',');
        }
    }
}
