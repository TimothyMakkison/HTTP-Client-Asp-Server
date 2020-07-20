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
            return Ok(Hasher(message, new SHA1Managed()));
        }

        [HttpGet("sha256")]
        public ActionResult<string> GetSha256([FromQuery] string message)
        {
            if (message == null)
            {
                return BadRequest("Bad Request");
            }
            return Ok(Hasher(message, new SHA256Managed()));
        }
        [HttpGet("getpublickey")]
        public ActionResult<string> GetPublicKey() => Ok(CryptoRSA.PublicKey);
        private string Hasher(string value, HashAlgorithm hashAlgorithm)
        {
            var plaintextBytes = Encoding.UTF8.GetBytes(value);
            var hashBytes = hashAlgorithm.ComputeHash(plaintextBytes);

            var sb = new StringBuilder();
            foreach (var hashByte in hashBytes)
            {
                sb.AppendFormat("{0:x2}", hashByte);
            }

            var hashString = sb.ToString();
            hashAlgorithm.Dispose();
            return hashString.ToUpper();
        }
    }
}
