using HTTP_Client_Asp_Server.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class AuthenticatedSender : BaseSender
    {

        public User User { get; set; }
        public AuthenticatedSender(HttpClient client, User user) : base(client)
        {
            User = user;
        }
        protected async virtual Task<HttpResponseMessage> SendAuthenticatedAsync(HttpRequestMessage request)
        {
            if (!User.Assigned)
            {
                Console.WriteLine("Client must get the locally stored username and ApiKey.If they don’t yet exist the console must print “You need to do a User Post or User Set first");
                return null;
            }
            request.Headers.Add("ApiKey", User.ApiKey);

            return await base.SendAsync(request);
        }
    }
}
