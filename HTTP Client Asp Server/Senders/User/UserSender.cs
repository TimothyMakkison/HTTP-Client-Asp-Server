using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Senders
{
    public class UserSender : AuthenticatedSender
    {
        public UserSender(HttpClient client, IOutput output, UserHandler userHandler) 
            : base(client, output, userHandler)
        {
        }

        [Command("User Get")]
        public async Task<string> GetUser(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"user/new?username={line}");
            var response = await SendAsync(request);
            var product = await GetResponseString(response);
            return product;
        }

        [Command("User Post")]
        public async Task<string> NewUser(string name)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "user/new")
            {
                Content = ToHttpContent(name)
            };

            HttpResponseMessage response = await SendAsync(request);
            var product = await GetResponseString(response);

            if (response.StatusCode is not HttpStatusCode.OK)
            {
                return product;
            }

            var jObject = JObject.Parse(product);

            var user = new User() 
            {
                Username = jObject["userName"].Value<string>(), 
                ApiKey = Guid.Parse(jObject["apiKey"].Value<string>()) 
            };

            UserHandler.Set(user);
            Console.WriteLine("Got API Key");
            return user.ToString();
        }

        [Command("User Set")]
        public string UserSet(string name, Guid guid)
        {
            var user = new User() { Username = name, ApiKey = guid };
            UserHandler.Set(user);
            return $"Stored {user}";
        }

        [Command("User Delete")]
        public async Task DeleteUser()
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, $"user/removeuser?username={UserHandler.Value.Username}");
            var response = await SendAuthenticatedAsync(request);
            var product = await GetResponseString(response);
            var success = Convert.ToBoolean(product);
            Console.WriteLine(success);
        }

        [Command("User Role")]
        public async Task ChangeRole(string username, string role)
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "user/changerole")
            {
                Content = ToHttpContent(new { username, role })
            };

            var response = await SendAuthenticatedAsync(request);
            Console.WriteLine(await GetResponseString(response));
        }
    }
}