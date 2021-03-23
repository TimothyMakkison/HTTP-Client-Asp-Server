using Client.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Senders
{
    public interface IAuthenticatedSender : ISender
    {
        UserHandler UserHandler { get; set; }
        Task<HttpResponseMessage> SendAuthenticatedAsync(HttpRequestMessage request);
        bool UserCheck();
    }
}