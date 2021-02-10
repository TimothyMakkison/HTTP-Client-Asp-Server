using HTTP_Client_Asp_Server.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class AuthenticatedSender : BaseSender
    {
        public UserHandler UserHandler { get; set; }

        public AuthenticatedSender(HttpClient client, UserHandler userHandler) : base(client)
        {
            UserHandler = userHandler;
        }

        protected bool UserCheck()
        {
            if (UserHandler.Assigned)
            {
                return true;
            }
            Console.WriteLine("You need to do a User Post or User Set first");
            return false;
        }

        protected async virtual Task<HttpResponseMessage> SendAuthenticatedAsync(HttpRequestMessage request)
        {
            request.Headers.Add("ApiKey", UserHandler.Value.ApiKey);
            return await base.SendAsync(request);
        }
    }
}