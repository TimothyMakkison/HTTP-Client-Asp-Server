using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Senders;

public interface ISender
{
    Task<string> GetResponseString(HttpResponseMessage response);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    HttpContent ToHttpContent(object item);
}
