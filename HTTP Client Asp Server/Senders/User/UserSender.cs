using HTTP_Client_Asp_Server.Handlers;
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
        public UserSender(HttpClient client, UserHandler userHandler) : base(client, userHandler)
        {
        }

        [Command("User Get")]
        public async Task GetUser(string line)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"user/new?username={line}");
            var response = await SendAsync(request);
            var product = await GetResponseString(response);
            Console.WriteLine(product);
        }

        [Command("User Post")]
        public async Task NewUser(string name)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "user/new")
            {
                Content = ToHttpContent(name)
            };

            HttpResponseMessage response = await SendAsync(request);
            var product = GetResponseString(response).Result;

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(product);
                return;
            }

            var jObject = JObject.Parse(product);
            var user = new User() { Username = jObject["userName"].Value<string>(), ApiKey = jObject["apiKey"].Value<string>() };
            UserHandler.Set(user);
            Console.WriteLine("Got API Key");
        }

        [Command("Hey")]
        public void Hey()
        {
            Console.WriteLine("Hey hey people");
        }

        [Command("User Set")]
        public async Task UserSet(string line)
        {
            // Input should be in the form "User Set <username> <apikey>"
            var parts = line.Split(' ');

            if (parts.Length <= 1)
            {
                Console.WriteLine("Invalid input, must contain a valid username and Guid");
                return;
            }

            var user = new User() { Username = string.Join(" ", parts.Take(parts.Length - 1)), ApiKey = parts.LastOrDefault() };
            UserHandler.Set(user);
            Console.WriteLine("Stored");
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
        public async Task ChangeRole(string line)
        {
            var parts = line.Split(' ');

            if (parts.Length <= 1)
            {
                Console.WriteLine("Invalid input, must contain a valid username and role");
                return;
            }

            var role = parts.LastOrDefault();
            var username = string.Join(" ", parts.Take(parts.Length - 1));

            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Post, "user/changerole")
            {
                Content = ToHttpContent(new { username, role })
            };

            var response = SendAuthenticatedAsync(request).Result;
            Console.WriteLine(GetResponseString(response).Result);
        }
    }
}