using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net_Core_Server.Data;
using Net_Core_Server.Encryption;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Net_Core_Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProtectedController : ControllerBase
    {
        private readonly UserDataAccess dataAccess;

        public ProtectedController(UserContext context) => dataAccess = new UserDataAccess(context);

        [HttpGet("hello")]
        public async Task<ActionResult<string>> GetHello()
        {
            var guid = Guid.Parse(Request.Headers["ApiKey"]);
            var user = await dataAccess.TryGet(guid);
            return Ok($"Hello {user.UserName}");
        }

        [HttpGet("sha1")]
        public ActionResult<string> GetSha1([FromQuery] string message)
        {
            if (message == null)
            {
                return BadRequest("Bad Request");
            }
            return Ok(CryptoServices.Hasher(message, new SHA1Managed()));
        }

        [HttpGet("sha256")]
        public ActionResult<string> GetSha256([FromQuery] string message)
        {
            return message == null
                ? BadRequest("Bad Request")
                : (ActionResult<string>)Ok(CryptoServices.Hasher(message, new SHA256Managed()));
        }

        [HttpGet("getPublicKey")]
        public ActionResult<string> GetPublicKey() => Ok(CryptoServices.RsaPublicKey);

        [HttpGet("sign")]
        public ActionResult<string> GetSignValue([FromQuery] string message)
        {
            if (message == null)
            {
                return BadRequest("Query must contain a value.");
            }
            var plaintextBytes = Encoding.UTF8.GetBytes(message);
            var signedData = CryptoServices.RSA.SignData(plaintextBytes, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

            var hexadecimal = BitConverter.ToString(signedData);
            return Ok(hexadecimal);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("addFifty")]
        public ActionResult<string> AddFifty([FromQuery] string encryptedInteger, string encryptedSymKey, string encryptedIV)
        {
            try
            {
                var symKey = DecryptHex(encryptedSymKey);
                var IV = DecryptHex(encryptedIV);

                var integerByteForm = DecryptHex(encryptedInteger);
                var integerString = Encoding.Default.GetString(integerByteForm);
                var integer = Convert.ToInt32(integerString);
                var returnInt = integer + 50;

                var returnEncrypted = CryptoServices.AesEncrypt(returnInt.ToString(), symKey, IV);
                var returnString = BitConverter.ToString(returnEncrypted);
                return Ok(returnString);
            }
            catch
            {
                return BadRequest("Bad Request");
            }

            static byte[] DecryptHex(string hex)
            {
                var byteform = CryptoServices.HexToByte(hex);
                return CryptoServices.RSA.Decrypt(byteform, true);
            }
        }
    }
}