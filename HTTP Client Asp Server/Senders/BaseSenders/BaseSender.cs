using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public abstract class BaseSender
    {
        protected HttpClient Client;

        public BaseSender(HttpClient client)
        {
            this.Client = client;
        }

        protected async virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var result = Client.SendAsync(request);
            Console.WriteLine("...please wait...");
            return await result;
        }

        protected async virtual Task<string> GetResponseString(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        protected virtual HttpContent ToHttpContent(object item)
        {
            var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            return new StringContent(stringContent, Encoding.UTF8, "application/json");
        }
    }
}