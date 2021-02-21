using HTTP_Client_Asp_Server.ConsoleClass;
using HTTP_Client_Asp_Server.Models;
using StructureMap;
using System;
using System.Net.Http;

namespace HTTP_Client_Asp_Server
{
    internal class Program
    {
        private static void Main()
        {
            string address = @"https://localhost:44391/api/";
            var client = new HttpClient
            {
                BaseAddress = new Uri(address)
            };

            var container = new Container(_ =>
            {
                _.ForSingletonOf<HttpClient>().Use(client);
                _.ForSingletonOf<UserHandler>();
                _.ForSingletonOf<CryptoKey>();
            });

            var handler = new CommandLineBuilder()
                .SetContainer(container)
                .AddCommand(new CommandModel("exit", new Action(() => Environment.Exit(0))))
                .Build();

            var console = new ConsoleHandler(handler);
            console.Run();
        }
    }
}
//TODO Add StructureMap like CommandBuilder constructor

//TODO 
