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
            const string address = @"https://localhost:44391/api/";
            CommandLineHandler handler = BuildHandler(address);

            var console = new ConsoleHandler(handler);
            console.Run();
        }

        private static CommandLineHandler BuildHandler(string address)
        {
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

            return new CommandLineBuilder()
                .SetContainer(container)
                .AddCommand(new CommandModel("exit", new Action(() => Environment.Exit(0))))
                .Build();
        }
    }
}
//TODO Add StructureMap like CommandBuilder constructor

//TODO 
