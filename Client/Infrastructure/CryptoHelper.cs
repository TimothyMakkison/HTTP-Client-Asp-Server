using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Client.Infrastructure
{
    public static class CryptoHelper
    {
        public static byte[] HexToByte(this string hex)
        {
            //Split along dashes and convert hex to bytes.
            return hex.Split("-")
                      .Select(x => Convert.ToByte(x, 16))
                      .ToArray();
        }

        public static string AesDecrypt(byte[] cipher, byte[] key, byte[] IV)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var msDecrypt = new MemoryStream(cipher);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }
    }
}