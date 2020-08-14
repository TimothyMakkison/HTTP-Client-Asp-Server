using HTTP_Client_Asp_Server.Models;
using HTTP_Client_Asp_Server.Senders;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
            var userHandler = new UserHandler();
            var serverKey = new CryptoKey();

            var classCollection = new object[]
            {
                new TalkBackHello(client),
                new TalkBackSort(client),
                new UserSender(client,userHandler),
                new ProtectedSender(client, userHandler, serverKey),
                new ProtectedSignMessage(client,userHandler,serverKey),
                new ProtectedAddFifty(client,userHandler,serverKey),
            };

            MethodFilter methodFilter = new MethodFilter
            {
                Filter = info => info.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0
            };

            var methods = methodFilter.GetValidMethods<Func<string, Task>>(classCollection);
            var comModel = methodFilter.ToCommandModel(methods);

            Console.WriteLine($"Loaded {comModel.Count()} commands.");
            return new ConsoleHandler(comModel);
        }
    }
}