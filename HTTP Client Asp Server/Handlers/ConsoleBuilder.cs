using HTTP_Client_Asp_Server.Models;
using HTTP_Client_Asp_Server.Senders;
using System;
using System.Net.Http;
using System.Reflection;
using System.Linq;
using System.Linq.Expressions;

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
            var serverKey = new CryptoKey();

            MethodFilter methodFilter = new MethodFilter
            {
                Filter = info => info.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0
            };

            var classCollection = new object[] 
            { 
                new TalkBackHello(client), 
                new TalkBackSort(client),
                new UserSender(client,userHandler),
                new ProtectedSender(client, userHandler, serverKey),
                new ProtectedSignMessage(client,userHandler,serverKey),
                new ProtectedAddFifty(client,userHandler,serverKey),

            };

            var methods = methodFilter.GetValidMethods<Action<string>>(classCollection).ToList();
            var comModel = methodFilter.ToCommandModel(methods);

            return new ConsoleHandler().AddRange(comModel);
        }
    }
}