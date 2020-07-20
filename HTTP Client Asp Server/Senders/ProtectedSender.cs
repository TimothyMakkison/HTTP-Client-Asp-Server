using HTTP_Client_Asp_Server.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Serialization;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedSender : AuthenticatedSender
    {
        public string ServerPublicKey { get; set; }
        public ProtectedSender(HttpClient client, User user) : base(client, user)
        {
        }

        public void ProtectedHello(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "protected/hello");
            var response = SendAuthenticatedAsync(request).Result;
            if (response == null)
            {
                return;
            }
            Console.WriteLine(GetResponseString(response).Result);
        }
        public void Sha1(string line)
        {
            var value = line.Replace("Protected SHA1 ", "");
            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha1?message={value}");
            var response = SendAuthenticatedAsync(request).Result;
            if (response == null)
            {
                return;
            }
            Console.WriteLine(GetResponseString(response).Result);
        }
        public void Sha256(string line)
        {
            var value = line.Replace("Protected SHA256 ", "");
            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha256?message={value}");
            var response = SendAuthenticatedAsync(request).Result;
            if (response == null)
            {
                return;
            }
            Console.WriteLine(GetResponseString(response).Result);
        }
        public async void GetPublicKey(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/getpublickey");
            var response = SendAuthenticatedAsync(request).Result;

            if(response == null)
            {
                return;
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("“Couldn’t Get the Public Key");
                return;
            }

            var content = await GetResponseString(response);
            Console.WriteLine("Got Public Key");
            ServerPublicKey = content;
        }
    }
}
