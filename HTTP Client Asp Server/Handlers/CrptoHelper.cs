using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HTTP_Client_Asp_Server.Handlers
{
    public static class CrptoHelper
    {
        public static byte[] AesEncrypt(object input, byte[] key, byte[] IV)
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using var swEncrypt = new StreamWriter(csEncrypt);

            swEncrypt.Write(input);
            return msEncrypt.ToArray();
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
