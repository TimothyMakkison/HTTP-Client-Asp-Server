using HTTP_Client_Asp_Server.Handlers;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Net;
using System.Net.Http;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedSender : AuthenticatedSender
    {
        private CryptoKey ServerPublicKey { get; set; }

        public ProtectedSender(HttpClient client, UserHandler userHandler, CryptoKey cryptoKey) : base(client, userHandler)
        {
            ServerPublicKey = cryptoKey;
        }


        [Command("Protected  Hello")]
        public void ProtectedHello(string line)
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "protected/hello");
            var response = SendAuthenticatedAsync(request).Result;
            Console.WriteLine(GetResponseString(response).Result);
        }

        [Command("Protected Sha1", Parsing = ParseMode.ParseAndTrim)]
        public void Sha1(string value)
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha1?message={value}");
            var response = SendAuthenticatedAsync(request).Result;
            Console.WriteLine(GetResponseString(response).Result);
        }

        [Command("Protected Sha256", Parsing = ParseMode.ParseAndTrim)]
        public async void Sha256(string value)
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha256?message={value}");
            var response = await SendAuthenticatedAsync(request);
            Console.WriteLine(await GetResponseString(response));
        }

        [Command("Protected Get PublicKey")]
        public async void GetPublicKey(string line)
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/getpublickey");
            var response = await SendAuthenticatedAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("“Couldn’t Get the Public Key");
                return;
            }

            var content = await GetResponseString(response);
            Console.WriteLine("Got Public Key");
            ServerPublicKey.Key = content;
        }
    }
}