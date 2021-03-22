using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackHello : BaseSender
    {
        public TalkBackHello(HttpClient client, IOutput output) 
            : base(client, output)
        {
        }

        [Command("TalkBack Hello")]
        public async Task<string> Hello()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "talkback/hello");
            HttpResponseMessage response = await SendAsync(request);
            return await base.GetResponseString(response);
        }
    }
}