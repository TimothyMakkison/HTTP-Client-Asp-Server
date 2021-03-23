using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public interface ISender
    {
        Task<string> GetResponseString(HttpResponseMessage response);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
        HttpContent ToHttpContent(object item);
    }
}