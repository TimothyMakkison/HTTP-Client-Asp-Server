using HTTP_Client_Asp_Server.Handlers;
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

        public ProtectedAddFifty(HttpClient client, UserHandler userHandler, CryptoKey cryptoKey) : base(client, userHandler)
        {
            ServerPublicKey = cryptoKey;
        }

        [Command("Protected AddFifty")]
        public async void Process(string value)
        {
            if (!HasKey())
            {
                return;
            }

            if (!int.TryParse(value, out int _))
            {
                Console.WriteLine("A valid integer must be given!");
                return;
            }

            using Aes aes = Aes.Create();
            var request = GenerateWebRequest(value, aes);
            HttpResponseMessage response = await SendAuthenticatedAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("An error occurred!");
                return;
            }

            string decrypted = await Decrypt(response, aes);
            Console.WriteLine(decrypted);
        }

        private HttpRequestMessage GenerateWebRequest(string value, Aes Aes)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(ServerPublicKey.Key);

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
            var encryptedBytes = CryptoHelper.HexToByte(encryptedHex);
            return CryptoHelper.AesDecrypt(encryptedBytes, aes.Key, aes.IV);
        }

        private bool HasKey()
        {
            if (ServerPublicKey.HasKey)
                return true;

            Console.WriteLine("Client doesn’t yet have the public key");
            return false;
        }
    }
}