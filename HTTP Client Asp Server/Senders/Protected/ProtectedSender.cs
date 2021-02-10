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

        public ProtectedSender(HttpClient client, UserHandler userHandler, CryptoKey cryptoKey) : base(client, userHandler)
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

        [Command("Protected Sha1")]
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

        [Command("Protected Sha256")]
        public async Task Sha256(string value)
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
        public async Task GetPublicKey()
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
            ServerPublicKey.Set(content);
        }
    }
}