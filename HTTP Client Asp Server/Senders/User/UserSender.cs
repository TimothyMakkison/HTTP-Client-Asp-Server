using HTTP_Client_Asp_Server.Handlers;
using HTTP_Client_Asp_Server.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace HTTP_Client_Asp_Server.Senders
{
    public class UserSender : AuthenticatedSender
    {
        public UserSender(HttpClient client, UserHandler userHandler) : base(client, userHandler)
        {
        }

        public async void GetUser(string line)
        {
            line = line.Replace("User Get ", "");

            var request = new HttpRequestMessage(HttpMethod.Get, $"user/new?username={line}");
            var response = await SendAsync(request);
            var product = await GetResponseString(response);
            Console.WriteLine(product);
        }

        public async void NewUser(string line)
        {
            var name = line.Replace("User Post ", "");
            var request = new HttpRequestMessage(HttpMethod.Post, "user/new")
            {
                Content = ToHttpContent(name)
            };

            HttpResponseMessage response = await SendAsync(request);
            var product = await GetResponseString(response);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(product);
                return;
            }

            var jObject = JObject.Parse(product);
            UserHandler.SetValues(jObject["userName"].Value<string>(), jObject["apiKey"].Value<string>());
            Console.WriteLine("Got API Key");
        }

        public void UserSet(string line)
        {
            // Input should be in the form "User Set <username> <apikey>"
            var values = line.Replace("User Set ", "");
            var parts = values.Split(' ');

            if (parts.Length <= 1)
            {
                Console.WriteLine("Invalid input, must contain a valid username and Guid");
                return;
            }
            UserHandler.SetValues(string.Join(" ", parts.Take(parts.Length - 1)), parts.LastOrDefault());
            Console.WriteLine("Stored");
        }

        public async void DeleteUser(string line)
        {
            if (!UserCheck())
            {
                return;
            }

            var request = new HttpRequestMessage(HttpMethod.Delete, $"user/removeuser?username={UserHandler.Username}");
            var response = await SendAuthenticatedAsync(request);
            var product = await GetResponseString(response);
            var success = Convert.ToBoolean(product);
            Console.WriteLine(success);
        }

        public void ChangeRole(string line)
        {
            line = line.Replace("User Role ", "");
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
                Content = ToHttpContent(new UserRolePair() { username = username, role = role })
            };

            var response = SendAuthenticatedAsync(request).Result;
            Console.WriteLine(GetResponseString(response).Result);
        }
    }
}