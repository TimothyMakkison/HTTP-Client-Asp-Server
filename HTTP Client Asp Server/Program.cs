using HTTP_Client_Asp_Server.Models;
using HTTP_Client_Asp_Server.Senders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace HTTP_Client_Asp_Server
{
    class Program
    {
        public static async void And()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44324/");
           
            var array = new int[] { 1, 45, 1, 24, 5, 2 };

            var serial = JsonConvert.SerializeObject(array);
            //var encode = new enco(array);

            var content = new StringContent(serial, Encoding.UTF8, "application/json");
            //Console.WriteLine(content.ToString());

            //HttpResponseMessage response = await client.PostAsync("api/talkback/sort",content);
            var response = client.PostAsJsonAsync("api/talkback/sort", content).Result;

            Console.WriteLine(response.ToString());
            var product = await response.Content.ReadAsStringAsync();
            Console.WriteLine(product);
        }
        //public static async void Builder()
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:44324/");
        //    var array = new int[] { 1, 45, 1, 24, 5, 2 };


        //    var builder = new UriBuilder("https://localhost:44324/");
        //    builder.Port = -1;
        //    var query = HttpUtility.ParseQueryString(builder.Query);
        //    query["foo"] = "bar<>&-baz";
        //    query["bar"] = "bazinga";
        //    builder.Query = query.ToString();
        //    string url = builder.ToString();

        //    var serial = JsonConvert.SerializeObject(array);
        //    //var encode = new enco(array);

        //    var content = new StringContent(serial, Encoding.UTF8, "application/json");
        //    //Console.WriteLine(content.ToString());

        //    HttpResponseMessage response = await client.PostAsync("api/talkback/sort", content);

        //    Console.WriteLine(response.ToString());
        //    var product = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine(product);
        //}

        static void Main()
        {
            ConsoleHandler ch = new ConsoleHandler();
            ch.Commands.AddRange(Builder.Commands());
            ch.Run();
        }
    }
}

public static class Builder
{
    public static List<CommandPair> Commands()
    {
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://localhost:44391/api/");

        var commands = new List<CommandPair>();

        User sharedUser = new User();
        var talkBack = new TalkBackSender(client);
        var userSender = new UserSender(client,sharedUser);
        var protectedSender = new ProtectedSender(client, sharedUser);

        commands.Add(new CommandPair("TalkBack Hello", talkBack.HelloWorld));
        commands.Add(new CommandPair("TalkBack Sort", talkBack.Sort));

        commands.Add(new CommandPair("User Get", userSender.GetUser));
        commands.Add(new CommandPair("User Post", userSender.NewUser));
        commands.Add(new CommandPair("User Delete", userSender.DeleteUser));
        commands.Add(new CommandPair("User Set", userSender.UserSet));
        commands.Add(new CommandPair("User Role", userSender.ChangeRole));

        commands.Add(new CommandPair("Protected Hello", protectedSender.ProtectedHello));
        commands.Add(new CommandPair("Protected SHA1", protectedSender.Sha1));
        commands.Add(new CommandPair("Protected SHA256", protectedSender.Sha256));
        commands.Add(new CommandPair("Protected Get PublicKey", protectedSender.GetPublicKey));

        return commands;
    }
}