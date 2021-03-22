using HTTP_Client_Asp_Server.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public interface IAuthenticatedSender : ISender
    {
        UserHandler UserHandler { get; set; }
        Task<HttpResponseMessage> SendAuthenticatedAsync(HttpRequestMessage request);
        bool UserCheck();
    }
}