using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Net_Core_Server.Data;
using Net_Core_Server.Models;
using Newtonsoft.Json.Linq;

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
        public async Task<ActionResult<string>> GetUser([FromQuery] string username)
        {
            string output = await dataAccess.ContainsUsername(username) ? "True - User Does Exist!" : "False - User Does Not Exist!"
                                                                   + " Did you mean to do a POST to create a new user?";
            return Ok(output);
        }
        [HttpPost("new")]
        public async Task<ActionResult<string>> PostNewUser([FromBody] string jsonString)
        {
            if (jsonString == null)
            {
                return BadRequest("Oops. Make sure your body contains a string with your username and your Content - Type is Content - Type:application / json");
            }
            var contains = await dataAccess.ContainsUsername(jsonString);
            if (contains)
            {
                return Forbid("Oops. This username is already in use. Please try again with a new username.");
            }
            else
            {
                return Ok(await dataAccess.AddNewUser(jsonString));
            }
        }
        [HttpDelete("RemoveUser")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<bool>> RemoveUser([FromQuery] string username)
        {
            var value = Request.Headers["ApiKey"];
            var user = await dataAccess.TryGet(Guid.Parse(value));

            return username == user.UserName || user.Role == Role.Admin
                ? Ok(await dataAccess.Remove(user.ApiKey)) 
                : (ActionResult<bool>)Ok(false);
        }
        [HttpPost("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> UpdateRole([FromBody] JObject jObject)
        {
            string username = jObject["username"].Value<string>();
            string role = jObject["role"].Value<string>();

            if (!(role == Role.Admin || role == Role.User))
            {
                return BadRequest("NOT DONE: Role does not exist");
            }
            if (await dataAccess.ChangeRole(username, role))
            {
                return Ok("DONE");
            }
            else
            {
                return BadRequest("NOT DONE: Username does not exist");
            }
            return BadRequest("NOT DONE: An error occured");
        }
    }
}
