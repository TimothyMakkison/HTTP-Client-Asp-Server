using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Net_Core_Server.Data;

namespace Net_Core_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly UserDataAccess dataAccess;
        public UserController(UserContext context)
        {
            dataAccess = new UserDataAccess(context);
        }

        [HttpGet("new")]
        public ActionResult<string> GetUser([FromQuery] string username)
        {

            string output = dataAccess.ContainsUsername(username) ? "True - User Does Exist!" : "False - User Does Not Exist!"
                                                                   + " Did you mean to do a POST to create a new user?";
            output = username == null ? "null" : output;
            return Ok(output);
        }
        [HttpPost("new")]
        public ActionResult<string> PostNewUser([FromBody] string jsonString)
        {
            if (jsonString == null)
            {
                return BadRequest("Oops. Make sure your body contains a string with your username and your Content - Type is Content - Type:application / json");
            }
            var contains = dataAccess.ContainsUsername(jsonString);
            if (contains)
            {
                return Forbid("Oops. This username is already in use. Please try again with a new username.");
            }
            else
            {
                return Ok(dataAccess.AddNewUser(jsonString));
            }
            
        }
    }
}
