using Asp_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Asp_Server.Filters;

namespace Asp_Server.Controllers
{
    public class UserController : ApiController
    {
        [ActionName("new")]
        [HttpGet]
        public IHttpActionResult GetContainsUser()
        {
            return Ok("False - User Does Not Exist! Did you mean to do a POST to create a new user?");
        }
        [ActionName("new")]
        [HttpGet]
        public IHttpActionResult GetContainsUser([FromUri] string username)
        {
            bool contains;
            using (UsersEntitiesConnection database = new UsersEntitiesConnection())
            {
                contains = database.Users.Any(person => person.UserName == username);
            }
            string output = contains ? "True - User Does Exist!" : "False - User Does Not Exist!"
                                                                   + " Did you mean to do a POST to create a new user?";

            return Ok(output);
        }

        [ActionName("new")]
        [HttpPost]
        [Auth(Roles ="")]
        public async Task<IHttpActionResult> PostNewUser()
        {
            var content  = Request.Content;
            var username = await content.ReadAsStringAsync();

            if(username == "")
            {
                return BadRequest("Oops. Make sure your body contains a string with+" +
                    " your username and your Content - Type is Content - Type:application / json");
            }
            using (UsersEntitiesConnection database = new UsersEntitiesConnection())
            {
                bool contains = database.Users.Any(person => person.UserName == username);
                if (contains)
                {
                    return Ok("Oops. This username is already in use. Please try again with a new username.");
                }
                else
                {
                    string role = database.Users.Count() == 0 ? "Admin" : "User";
                    var newUser = new User() { UserName = username, Role = role };
                    database.Users.Add(newUser);
                    return Ok(newUser.ApiKey.ToString());
                }

            }
        }
    }
}
