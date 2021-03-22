using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedSender : AuthenticatedSender
    {
        private CryptoKey ServerPublicKey { get; set; }

        public ProtectedSender(HttpClient client, IOutput output, UserHandler userHandler, CryptoKey cryptoKey) : base(client, output, userHandler)
        {
            ServerPublicKey = cryptoKey;
        }

        [Command("Protected Hello")]
        public string ProtectedHello()
        {
            if (UserCheck())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "protected/hello");
                var response = SendAuthenticatedAsync(request).Result;
                return GetResponseString(response).Result;
            }
            return "";
        }

        [Command("Protected SHA1")]
        public async Task Sha1(string message)
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha1?message={message}");
            var response = await SendAuthenticatedAsync(request);
            Console.WriteLine(await GetResponseString(response));
        }

        [Command("Protected SHA256")]
        public async Task Sha256(string message)
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha256?message={message}");
            var response = await SendAuthenticatedAsync(request);
            Console.WriteLine(await GetResponseString(response));
        }

        [Command("Protected Get PublicKey")]
        public async Task GetPublicKey()
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/getpublickey");
            var response = await SendAuthenticatedAsync(request);

            if (response.StatusCode is not HttpStatusCode.OK)
            {
                Console.WriteLine("“Couldn’t Get the Public Key");
                return;
            }

            var content = await GetResponseString(response);
            ServerPublicKey.Set(content);
            Console.WriteLine("Got Public Key");
        }
    }
}