using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class ProtectedSignMessage
    {
        private readonly ILogger _output;
        private readonly CryptoKey _serverPublicKey;
        private readonly IAuthenticatedSender _sender;

        public ProtectedSignMessage(ILogger output, CryptoKey cryptoKey, IAuthenticatedSender sender)
        {
            _output = output;
            _serverPublicKey = cryptoKey;
            _sender = sender;
        }

        [Command("Protected Sign")]
        public async Task Process(string value)
        {
            if (!HasKey())
            {
                return;
            }

            if (!_sender.UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"protected/sign?message={value}");
            var response = _sender.SendAuthenticatedAsync(request);

            // Hash while waiting for server response
            var hashedValue = Hash(value);

            // Convert hex string to byte array;
            var hexadecimal = await _sender.GetResponseString(await response);
            var signatureHash = CryptoHelper.HexToByte(hexadecimal);
            bool validSignature = VerifyHash(hashedValue, signatureHash);

            string outcome = validSignature
                ? "Message was successfully signed"
                : "Message was not successfully signed";
            _output.Log(outcome);
        }

        private bool VerifyHash(byte[] hash, byte[] signature)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(_serverPublicKey.Value);
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
            if (_serverPublicKey.Assigned)
                return true;

            _output.Log("Client doesn’t yet have the public key", LogType.Warning);
            return false;
        }
    }
}