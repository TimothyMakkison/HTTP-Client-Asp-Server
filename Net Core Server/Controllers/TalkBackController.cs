using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net_Core_Server.Data;


namespace Net_Core_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalkBackController : ControllerBase
    {
        [HttpGet("hello")]
        public ActionResult<string> GetHello()
        {
            return Ok("Hello");
        }
        [HttpGet("sort")]
        public ActionResult<int[]> GetSort([FromQuery] List<int> integer)
        {
            if (integer == null)
            {
                return BadRequest($"Please input parameters");
            }
            var sorted = integer.OrderBy(x => x);
            return Ok(sorted);
        }
    }
}
