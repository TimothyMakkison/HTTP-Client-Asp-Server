using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Net_Core_Server.Encryption
{
    public static class CryptoServices
    {
        public static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        public static string PublicKey => RSA.ToXmlString(false);
        public static byte[] Sha1Sign(byte[] payload)
        {
            return RSA.SignData(payload, HashAlgorithmName.SHA1);
        }
        public static string Hasher(string value, HashAlgorithm hashAlgorithm)
        {
            using (hashAlgorithm)
            {
                var plaintextBytes = Encoding.UTF8.GetBytes(value); // Convert to bytes
                var hashBytes = hashAlgorithm.ComputeHash(plaintextBytes); // Hash bytes with given algorithm
                var hexadecimal = BitConverter.ToString(hashBytes).Replace("-",string.Empty); // convert to hex

                return hexadecimal;
            }
        }
    }
}
