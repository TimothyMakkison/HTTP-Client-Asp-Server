using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class AuthenticatedSender : Sender, IAuthenticatedSender
    {
        public UserHandler UserHandler { get; set; }

        public AuthenticatedSender(HttpClient client, I output, UserHandler userHandler) : base(client, output)
        {
            UserHandler = userHandler;
        }

        public bool UserCheck()
        {
            if (UserHandler.Assigned)
            {
                return true;
            }
            Output.Log("You need to do a User Post or User Set first", LogType.Warning);
            return false;
        }

        public async Task<HttpResponseMessage> SendAuthenticatedAsync(HttpRequestMessage request)
        {
            request.Headers.Add("ApiKey", UserHandler.Value.ApiKey.ToString());
            return await SendAsync(request);
        }
    }
}