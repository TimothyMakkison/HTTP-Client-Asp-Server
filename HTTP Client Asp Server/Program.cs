using HTTP_Client_Asp_Server.Models;
using HTTP_Client_Asp_Server.Senders;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HTTP_Client_Asp_Server
{
    internal class Program
    {
        private static void Main()
        {
            ConsoleHandler ch = new ConsoleHandler();
            ch.Commands.AddRange(Commands("https://localhost:44391/api/"));
            ch.Run();
        }
        private static List<CommandPair> Commands(string baseAddress)
        {
            HttpClient client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            User sharedUser = new User();
            var userSender = new UserSender(client, sharedUser);
            var serverPublicKey = new CryptoKey();
            var protectedSender = new ProtectedSender(client, sharedUser, serverPublicKey);

            var commands = new List<CommandPair>
        {
            new CommandPair("TalkBack Hello", new TalkBackHello(client).Process),
            new CommandPair("TalkBack Sort", new TalkBackSort(client).Process),

            new CommandPair("User Get", userSender.GetUser),
            new CommandPair("User Post", userSender.NewUser),
            new CommandPair("User Delete", userSender.DeleteUser),
            new CommandPair("User Set", userSender.UserSet),
            new CommandPair("User Role", userSender.ChangeRole),

            new CommandPair("Protected Hello", protectedSender.ProtectedHello),
            new CommandPair("Protected SHA1", protectedSender.Sha1),
            new CommandPair("Protected SHA256", protectedSender.Sha256),
            new CommandPair("Protected Get PublicKey", protectedSender.GetPublicKey),
            new CommandPair("Protected Sign", new ProtectedSignMessage(client,sharedUser,serverPublicKey).Process),
            new CommandPair("Protected AddFifty", new ProtectedAddFifty(client,sharedUser,serverPublicKey).Process),
        };

            return commands;
        }
    }
}