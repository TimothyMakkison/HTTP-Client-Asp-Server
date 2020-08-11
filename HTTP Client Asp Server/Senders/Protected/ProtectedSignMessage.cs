using HTTP_Client_Asp_Server.Handlers;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedSignMessage : AuthenticatedSender
    {
        private CryptoKey ServerPublicKey { get; set; }

        public ProtectedSignMessage(HttpClient client, UserHandler userHandler, CryptoKey cryptoKey) : base(client, userHandler)
        {
            ServerPublicKey = cryptoKey;
        }

        public async void Process(string line)
        {
            if (!HasKey() || !UserCheck())
            {
                return;
            }

            var value = line.Replace("Protected Sign ", "");
            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sign?message={value}");
            var taskResponse = SendAuthenticatedAsync(request);

            // Hash while waiting for server response
            var hashedValue = Hash(value);
            var response = await taskResponse;

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
            rsa.FromXmlString(ServerPublicKey.Key);
            return rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        }

        private byte[] Hash(string value)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(value);
            using var hashAlgorithm = new SHA1Managed();
            return hashAlgorithm.ComputeHash(plaintextBytes);
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