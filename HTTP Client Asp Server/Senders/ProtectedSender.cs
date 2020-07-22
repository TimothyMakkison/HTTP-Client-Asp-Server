using HTTP_Client_Asp_Server.Handlers;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedSender : AuthenticatedSender
    {
        public string ServerPublicKey { get; set; }
        public ProtectedSender(HttpClient client, User user) : base(client, user)
        {
        }

        public void ProtectedHello(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "protected/hello");
            var response = SendAuthenticatedAsync(request).Result;
            if (response == null)
            {
                return;
            }
            Console.WriteLine(GetResponseString(response).Result);
        }
        public void Sha1(string line)
        {
            var value = line.Replace("Protected SHA1 ", "");
            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha1?message={value}");
            var response = SendAuthenticatedAsync(request).Result;
            if (response == null)
            {
                return;
            }
            Console.WriteLine(GetResponseString(response).Result);
        }
        public void Sha256(string line)
        {
            var value = line.Replace("Protected SHA256 ", "");
            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sha256?message={value}");
            var response = SendAuthenticatedAsync(request).Result;
            if (response == null)
            {
                return;
            }
            Console.WriteLine(GetResponseString(response).Result);
        }
        public async void GetPublicKey(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/getpublickey");
            var response = SendAuthenticatedAsync(request).Result;

            if(response == null)
            {
                return;
            }
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("“Couldn’t Get the Public Key");
                return;
            }

            var content = await GetResponseString(response);
            Console.WriteLine("Got Public Key");
            ServerPublicKey = content;
        }
        public async void SignMessage(string line)
        {
            if(!HasKey())
            {
                return;
            }

            var value = line.Replace("Protected Sign ","");
            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sign?message={value}");
            var taskResponse = SendAuthenticatedAsync(request);

            // Hash while waiting for server response
            var plaintextBytes = Encoding.UTF8.GetBytes(value); // Convert to bytes
            using var hashAlgorithm = new SHA1Managed();
            var hashBytes = hashAlgorithm.ComputeHash(plaintextBytes); // Hash bytes with given algorithm

            var response = await taskResponse;
            if (response == null)
            {
                return;
            }

            // Convert hex string to byte array;
            var hexadecimal = GetResponseString(response).Result;
            var clean = hexadecimal.Split("-");
            var signedData = clean.Select(x => Convert.ToByte(x, 16)).ToArray();

            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(ServerPublicKey);
            var similar = rsa.VerifyHash(hashBytes, signedData, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

            string outcome = similar ? "Message was successfully signed" : "Message was not successfully signed";
            Console.WriteLine(outcome);
        }

        public async void AddFifty(string line)
        {
            if (!HasKey())
            {
                return;
            }

            var value = line.Replace("Protected AddFifty ", string.Empty);
            if (!int.TryParse(value, out int _))
            {
                Console.WriteLine("A valid integer must be given!");
                return;
            }

            using Aes Aes = Aes.Create();
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(ServerPublicKey);

            byte[] valueBytes = Encoding.UTF8.GetBytes(value); // Convert to bytes
            string encryptValue = toEncryptedHex(valueBytes);
            string encryptKey = toEncryptedHex(Aes.Key);
            string encryptIV = toEncryptedHex(Aes.IV);

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/addfifty?encryptedInteger={encryptValue}" +
                $"&encryptedSymKey={encryptKey}&encryptedIV={encryptIV}");
            HttpResponseMessage response = await SendAuthenticatedAsync(request);

            if(response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("An error occurred!");
                return;
            }

            var encryptedHex = await GetResponseString(response);
            var encryptedBytes = CryptoHelper.HexToByte(encryptedHex);
            var decrypted = CryptoHelper.AesDecrypt(encryptedBytes, Aes.Key, Aes.IV);

            Console.WriteLine(decrypted);


            string toEncryptedHex (byte[] input)
            {
                byte[] encrypted = rsa.Encrypt(input, true);
                return BitConverter.ToString(encrypted);
            }
        }

        private bool HasKey()
        {
            if (ServerPublicKey != null || ServerPublicKey != "")
                return true;

            Console.WriteLine("Client doesn’t yet have the public key");
            return false;
        }
    }
}
