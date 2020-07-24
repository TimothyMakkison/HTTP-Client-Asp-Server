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
        public UserSender(HttpClient client, User user) : base(client, user)
        {
        }

        public void GetUser(string line)
        {
            line = line.Replace("User Get ", "");

            var request = new HttpRequestMessage(HttpMethod.Get, $"user/new?username={line}");
            var response = SendAsync(request).Result;
            var product = GetResponseString(response).Result;
            Console.WriteLine(product);
        }

        public void NewUser(string line)
        {
            var name = line.Replace("User Post ", "");

            var request = new HttpRequestMessage(HttpMethod.Post, "user/new")
            {
                Content = ToHttpContent(name)
            };

            HttpResponseMessage response = SendAsync(request).Result;
            var product = GetResponseString(response).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jObject = JObject.Parse(product);
                User.Username = jObject["userName"].Value<string>();
                User.ApiKey = jObject["apiKey"].Value<string>();
                User.Assigned = true;

                Console.WriteLine("Got API Key");
            }
            else
            {
                Console.WriteLine(product);
            }
        }

        public void UserSet(string line)
        {
            var values = line.Replace("User Set ", "");
            var parts = values.Split(' ');

            if (parts.Length <= 1)
            {
                Console.WriteLine("Invalid input, must contain a valid username and Guid");
                return;
            }
            User.ApiKey = parts.LastOrDefault();
            User.Username = string.Join(" ", parts.Take(parts.Length - 1));
            User.Assigned = true;
        }

        public void DeleteUser(string line)
        {
            string value = line.Replace("User Remove ", "");
            var request = new HttpRequestMessage(HttpMethod.Delete, $"user/removeuser?username={value}");
            var response = SendAuthenticatedAsync(request).Result;
            if (response == null)
            {
                return;
            }

            var product = GetResponseString(response).Result;
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

            var request = new HttpRequestMessage(HttpMethod.Post, "user/changerole")
            {
                Content = ToHttpContent(new UserRolePair() { username = username, role = role })
            };

            var response = SendAuthenticatedAsync(request).Result;
            if (response == null)
            {
                return;
            }
            Console.WriteLine(GetResponseString(response).Result);
        }
    }
}