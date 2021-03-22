using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedAddFifty
    {
        private readonly IOutput _output;
        private readonly CryptoKey _serverPublicKey;
        private readonly IAuthenticatedSender _sender;

        public ProtectedAddFifty(IOutput output, CryptoKey cryptoKey, IAuthenticatedSender sender)
        {
            _output = output;
            _serverPublicKey = cryptoKey;
            _sender = sender;
        }

        [Command("Protected AddFifty")]
        public async Task Process(string value)
        {
            //TODO change value to int and allow auto converter to handle.
            if (!HasKey())
            {
                return;
            }

            if (!int.TryParse(value, out int _))
            {
                _output.Log("A valid integer must be given!", LogType.Warning);
                return;
            }

            using Aes aes = Aes.Create();
            var request = GenerateWebRequest(value, aes);
            HttpResponseMessage response = await _sender.SendAuthenticatedAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                _output.Log("An error occurred!", LogType.Warning);
                return;
            }

            string decrypted = await Decrypt(response, aes);
            _output.Print(decrypted);
        }

        private HttpRequestMessage GenerateWebRequest(string value, Aes Aes)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_serverPublicKey.Value);

            // Convert value to bytes then
            byte[] valueBytes = Encoding.UTF8.GetBytes(value);
            string encryptValue = toEncryptedHex(valueBytes);
            string encryptKey = toEncryptedHex(Aes.Key);
            string encryptIV = toEncryptedHex(Aes.IV);

            return new HttpRequestMessage(HttpMethod.Get, $"protected/addfifty?encryptedInteger={encryptValue}" +
                $"&encryptedSymKey={encryptKey}&encryptedIV={encryptIV}");

            string toEncryptedHex(byte[] input)
            {
                byte[] encrypted = rsa.Encrypt(input, true);
                return BitConverter.ToString(encrypted);
            }
        }

        private async Task<string> Decrypt(HttpResponseMessage response, Aes aes)
        {
            var encryptedHex = await _sender.GetResponseString(response);
            var encryptedBytes = encryptedHex.HexToByte();
            return CryptoHelper.AesDecrypt(encryptedBytes, aes.Key, aes.IV);
        }

        private bool HasKey()
        {
            if (_serverPublicKey.Assigned)
                return true;

            _output.Log("Client doesn’t yet have the public key", LogType.Warning);
            return false;
        }
    }
}