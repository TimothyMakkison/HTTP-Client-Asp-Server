using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Net_Core_Server.Encryption;

public static class CryptoServices
{
    public readonly static RSACryptoServiceProvider RSA = new();

    public static string RsaPublicKey => RSA.ToXmlString(false);

    public static byte[] HexToByte(string hex)
    {
        string[] hexCollection = hex.Split("-");
        var r = hexCollection.Select(x => Convert.ToByte(x, 16)).ToArray();
        return r;
    }

    public static string Hasher(string value, HashAlgorithm hashAlgorithm)
    {
        using (hashAlgorithm)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(value); // Convert to bytes
            var hashBytes = hashAlgorithm.ComputeHash(plaintextBytes); // Hash bytes with given algorithm
            var hexadecimal = BitConverter.ToString(hashBytes).Replace("-", string.Empty); // convert to hex

            return hexadecimal;
        }
    }

    public static byte[] AesEncrypt(string input, byte[] key, byte[] IV)
    {
        using Aes aes = Aes.Create();
        aes.Key = key;
        aes.IV = IV;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            //Write all data to the stream.
            swEncrypt.Write(input);
        }
        return msEncrypt.ToArray();
    }
}
