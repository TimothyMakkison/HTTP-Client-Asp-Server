using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedSignMessage : AuthenticatedSender
    {
        private CryptoKey ServerPublicKey { get; set; }

        public ProtectedSignMessage(HttpClient client, IOutput output, UserHandler userHandler, CryptoKey cryptoKey) 
            : base(client, output, userHandler)
        {
            ServerPublicKey = cryptoKey;
        }

        [Command("Protected Sign")]
        public async Task Process(string value)
        {
            if (!HasKey() || !UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sign?message={value}");
            var response = await SendAuthenticatedAsync(request);

            // Hash while waiting for server response
            var hashedValue = Hash(value);

            // Convert hex string to byte array;
            var hexadecimal = GetResponseString(response).Result;
            var signatureHash = CryptoHelper.HexToByte(hexadecimal);
            bool validSignature = VerifyHash(hashedValue, signatureHash);

            string outcome = validSignature ? "Message was successfully signed" : "Message was not successfully signed";
            Console.WriteLine(outcome);
        }

        private bool VerifyHash(byte[] hash, byte[] signature)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(ServerPublicKey.Value);
            return rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        private static byte[] Hash(string value)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(value);
            using var hashAlgorithm = new SHA1Managed();
            return hashAlgorithm.ComputeHash(plaintextBytes);
        }

        private bool HasKey()
        {
            if (ServerPublicKey.Assigned)
                return true;

            Console.WriteLine("Client doesn’t yet have the public key");
            return false;
        }
    }
}