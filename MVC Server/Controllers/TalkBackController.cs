using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalkBackController : ControllerBase
    {
        //[ActionName("hello")]

        [HttpGet("hello")]
        public ActionResult<string> GetHello()
        {
            return Ok("Hello");
        }

        //[ActionName("sort")]
        [HttpGet("sort")]
        public ActionResult<int[]> GetSort([FromQuery] List<int> integer)
        {

            if (integer == null)
            {
                return BadRequest($"Please input parameters");
            }
            var sorted = integer.OrderBy(x => x);
            return Ok(sorted);
            //return Ok(sorted);
        }
    }
}
