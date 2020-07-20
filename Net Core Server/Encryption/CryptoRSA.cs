using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Net_Core_Server.Encryption
{
    public static class CryptoRSA
    {
        public static RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

        public static string PublicKey => RSA.ToXmlString(false);
    }
}
