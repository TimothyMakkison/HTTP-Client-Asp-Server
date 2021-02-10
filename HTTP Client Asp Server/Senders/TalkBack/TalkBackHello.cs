using HTTP_Client_Asp_Server.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackHello : BaseSender
    {
        public TalkBackHello(HttpClient client) : base(client)
        {
        }

        [Command("TalkBack Hello")]
        public async Task<string> Hello()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "talkback/hello");
            HttpResponseMessage response = await SendAsync(request);
            var product = await GetResponseString(response);
            return product;
        }
    }
}