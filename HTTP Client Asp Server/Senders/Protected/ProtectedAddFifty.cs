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
        private readonly I _output;
        private readonly CryptoKey _serverPublicKey;
        private readonly IAuthenticatedSender _sender;

        public ProtectedAddFifty(I output, CryptoKey cryptoKey, IAuthenticatedSender sender)
        {
            _output = output;
            _serverPublicKey = cryptoKey;
            _sender = sender;
        }

        [Command("Protected AddFifty")]
        public async Task Process(int value)
        {
            //TODO change value to int and allow auto converter to handle.
            if (!HasKey())
            {
                return;
            }

            using Aes aes = Aes.Create();
            var request = GenerateWebRequest(value, aes);
            HttpResponseMessage response = await _sender.SendAuthenticatedAsync(request);

            if (response.StatusCode is not HttpStatusCode.OK)
            {
                _output.Log("An error occurred!", LogType.Warning);
                return;
            }

            string decrypted = await Decrypt(response, aes);
            _output.Print(decrypted);
        }

        private HttpRequestMessage GenerateWebRequest(int value, Aes Aes)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_serverPublicKey.Value);

            // Convert value to bytes then add to uri
            byte[] valueBytes = Encoding.UTF8.GetBytes(value.ToString());
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