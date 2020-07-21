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
        HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44391/api/")
        };

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
        commands.Add(new CommandPair("Protected Sign", protectedSender.SignMessage));
        

        return commands;
    }
}