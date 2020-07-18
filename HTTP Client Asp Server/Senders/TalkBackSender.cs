using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HTTP_Client_Asp_Server.Senders
{
    public class TalkBackSender : ITalkBackSender
    {
        public TalkBackSender(HttpClient client)
        {
            Client = client;
        }
        public HttpClient Client { get; set; }
    }
}
