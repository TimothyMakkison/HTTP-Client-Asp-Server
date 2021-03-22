using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedSender
    {
        private readonly IOutput _output;
        private readonly CryptoKey _serverPublicKey;
        private readonly IAuthenticatedSender _sender;

        public ProtectedSender(IOutput output, CryptoKey cryptoKey, IAuthenticatedSender sender)
        {
            _output = output;
            _serverPublicKey = cryptoKey;
            _sender = sender;
        }

        [Command("Protected Hello")]
        public string ProtectedHello()
        {
            if (!_sender.UserCheck())
            {
                return "";
            }
            var request = new HttpRequestMessage(HttpMethod.Get, "protected/hello");
            var response = _sender.SendAuthenticatedAsync(request).Result;
            return _sender.GetResponseString(response).Result;
        }

        [Command("Protected SHA1")]
        public async Task Sha1(string message)
        {
            if (!_sender.UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha1?message={message}");
            var response = await _sender.SendAuthenticatedAsync(request);
            _output.Log(await _sender.GetResponseString(response));
        }

        [Command("Protected SHA256")]
        public async Task Sha256(string message)
        {
            if (!_sender.UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha256?message={message}");
            var response = await _sender.SendAuthenticatedAsync(request);
            _output.Log(await _sender.GetResponseString(response));
        }

        [Command("Protected Get PublicKey")]
        public async Task GetPublicKey()
        {
            if (!_sender.UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/getpublickey");
            var response = await _sender.SendAuthenticatedAsync(request);

            if (response.StatusCode is not HttpStatusCode.OK)
            {
                _output.Log("Couldn’t Get the Public Key", LogType.Warning);
                return;
            }

            var content = await _sender.GetResponseString(response);
            _serverPublicKey.Set(content);
            _output.Log("Got Public Key");
        }
    }
}