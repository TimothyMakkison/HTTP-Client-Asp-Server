using HTTP_Client_Asp_Server.Infrastructure;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class Sender : ISender
    {
        protected HttpClient Client;
        protected ILogger Output;

        public Sender(HttpClient client, ILogger output)
        {
            this.Client = client;
            this.Output = output;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            var result = Client.SendAsync(request);
            Output.Log("...please wait...");
            return await result;
        }

        public async Task<string> GetResponseString(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }

        public HttpContent ToHttpContent(object item)
        {
            var stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(item);
            return new StringContent(stringContent, Encoding.UTF8, "application/json");
        }
    }
}