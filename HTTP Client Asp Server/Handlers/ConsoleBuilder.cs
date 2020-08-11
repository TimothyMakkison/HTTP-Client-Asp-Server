using HTTP_Client_Asp_Server.Models;
using HTTP_Client_Asp_Server.Senders;
using System;
using System.Net.Http;

namespace HTTP_Client_Asp_Server.Handlers
{
    public class ConsoleBuilder
    {
        private HttpClient client;

        public ConsoleBuilder(string baseAddress)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public ConsoleHandler BuildConsole()
        {
            UserHandler userHandler = new UserHandler();
            var userSender = new UserSender(client, userHandler);
            var serverPublicKey = new CryptoKey();
            var protectedSender = new ProtectedSender(client, userHandler, serverPublicKey);

            return new ConsoleHandler().Add(new CommandPair("TalkBack Hello", new TalkBackHello(client).Process))
                                       .Add(new CommandPair("TalkBack Sort", new TalkBackSort(client).Process))
                                       .Add(new CommandPair("User Get", userSender.GetUser))
                                       .Add(new CommandPair("User Post", userSender.NewUser))
                                       .Add(new CommandPair("User Delete", userSender.DeleteUser))
                                       .Add(new CommandPair("User Set", userSender.UserSet))
                                       .Add(new CommandPair("User Role", userSender.ChangeRole))
                                       .Add(new CommandPair("Protected Hello", protectedSender.ProtectedHello))
                                       .Add(new CommandPair("Protected SHA1", protectedSender.Sha1))
                                       .Add(new CommandPair("Protected SHA256", protectedSender.Sha256))
                                       .Add(new CommandPair("Protected Get PublicKey", protectedSender.GetPublicKey))
                                       .Add(new CommandPair("Protected Sign", new ProtectedSignMessage(client, userHandler, serverPublicKey).Process))
                                       .Add(new CommandPair("Protected AddFifty", new ProtectedAddFifty(client, userHandler, serverPublicKey).Process));
        }
    }
}