using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Net_Core_Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TalkBackController : ControllerBase
{
    [HttpGet("hello")]
    public ActionResult<string> GetHello() => Ok("Hello World");

    [HttpGet("sort")]
    public ActionResult<int[]> GetSort([FromQuery] List<int> integers)
    {
        return integers is null
                   ? BadRequest($"Please input parameters")
                   : (ActionResult<int[]>)Ok(integers.OrderBy(x => x));
    }
}
