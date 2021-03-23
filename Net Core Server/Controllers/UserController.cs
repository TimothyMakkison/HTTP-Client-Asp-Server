using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net_Core_Server.Data;
using Net_Core_Server.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Net_Core_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserDataAccess dataAccess;

        public UserController(UserContext context)
        {
            dataAccess = new UserDataAccess(context);
        }

        [HttpGet("new")]
        public async Task<ActionResult<string>> GetUser([FromQuery] string username)
        {
            string output = await dataAccess.ContainsUsername(username)
                ? "True - User Does Exist!"
                : "False - User Does Not Exist! Did you mean to do a POST to create a new user?";
            return Ok(output);
        }

        [HttpPost("new")]
        public async Task<ActionResult<string>> PostNewUser([FromBody] string jsonString)
        {
            if (jsonString == null)
            {
                return BadRequest("Oops. Make sure your body contains a string with your username and your Content - Type is Content - Type:application / json");
            }

            return await dataAccess.ContainsUsername(jsonString)
                ? Forbid("Oops. This username is already in use. Please try again with a new username.")
                : (ActionResult<string>)Ok(await dataAccess.Add(jsonString));
        }

        [HttpDelete("RemoveUser")]
        [Authorize]
        public async Task<ActionResult<bool>> RemoveUser([FromQuery] string username)
        {
            string actualUsername = User.FindFirstValue(ClaimTypes.Name);

            if (username == actualUsername)
            {
                var apiKeyString = Request.Headers["ApiKey"].ToString();
                var apiKey = Guid.Parse(apiKeyString);

                return Ok(await dataAccess.Remove(apiKey));
            }
            return Ok(false);
        }

        [HttpPost("ChangeRole")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> UpdateRole([FromBody] JObject jObject)
        {
            string username = jObject["username"].Value<string>();
            string role = jObject["role"].Value<string>();

            if (role == Role.Admin || role == Role.User)
            {
                return await dataAccess.ChangeRole(username, role)
                    ? Ok("DONE")
                    : (ActionResult<string>)BadRequest("NOT DONE: Username does not exist");
            }
            return BadRequest("NOT DONE: Role does not exist");
        }
    }
}