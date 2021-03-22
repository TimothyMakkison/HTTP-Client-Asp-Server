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
    public class ProtectedAddFifty : AuthenticatedSender
    {
        private CryptoKey ServerPublicKey { get; set; }

        public ProtectedAddFifty(HttpClient client, IOutput output, UserHandler userHandler, CryptoKey cryptoKey) : base(client, output ,userHandler)
        {
            ServerPublicKey = cryptoKey;
        }

        [Command("Protected AddFifty")]
        public async Task Process(string value)
        {
            if (!HasKey())
            {
                return;
            }

            if (!int.TryParse(value, out int _))
            {
                Output.Log("A valid integer must be given!", LogType.Warning);
                return;
            }

            using Aes aes = Aes.Create();
            var request = GenerateWebRequest(value, aes);
            HttpResponseMessage response = await SendAuthenticatedAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Output.Log("An error occurred!", LogType.Warning);
                return;
            }

            string decrypted = await Decrypt(response, aes);
            Output.Print(decrypted);
        }

        private HttpRequestMessage GenerateWebRequest(string value, Aes Aes)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(ServerPublicKey.Value);

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
            var encryptedHex = await GetResponseString(response);
            var encryptedBytes = encryptedHex.HexToByte();
            return CryptoHelper.AesDecrypt(encryptedBytes, aes.Key, aes.IV);
        }

        private bool HasKey()
        {
            if (ServerPublicKey.Assigned)
                return true;

            Output.Log("Client doesn’t yet have the public key",LogType.Warning);
            return false;
        }
    }
}