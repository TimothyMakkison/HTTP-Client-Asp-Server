using System;
using System.Net.Http;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackHello : BaseSender
    {
        public TalkBackHello(HttpClient client) : base(client)
        {
        }

        public async void Process(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "talkback/hello");
            HttpResponseMessage response = await SendAsync(request);
            var product = await GetResponseString(response);
            Console.WriteLine(product);
        }
    }
}