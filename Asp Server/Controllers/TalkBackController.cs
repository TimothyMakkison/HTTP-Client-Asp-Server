using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Asp_Server.Models;
using Newtonsoft.Json;

namespace Asp_Server.Controllers
{
    public class TalkBackController : ApiController
    {
        [ActionName("hello")]
        [HttpGet]
        public IHttpActionResult GetHello()
        {
            return Ok("Hello");
        }

        [ActionName("sort")]
        [HttpGet]
        public IHttpActionResult GetSort([FromUri] List<int> integer)
        {
            if (integer == null)
            {
                return BadRequest($"Please input parameters");
            }
            var sorted = integer.OrderBy(x => x);
            return Ok(JsonConvert.SerializeObject(sorted));
            //return Ok(sorted);
        }
    }
}
