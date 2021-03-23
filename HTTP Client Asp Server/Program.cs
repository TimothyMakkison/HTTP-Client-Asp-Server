﻿using HTTP_Client_Asp_Server.ConsoleClass;
using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using HTTP_Client_Asp_Server.Senders;
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
            var console = new ConsoleHandler(new CommandLineHandler(address));
            console.Run();
        }

        private static CommandLineHandler BuildHandler(string address, ConsoleOutput consoleOutput)
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
                _.ForSingletonOf<ILogger>().Use(consoleOutput);
                _.For<IAuthenticatedSender>().Add<AuthenticatedSender>();
                _.For<ISender>().Add<Sender>();
            });

            return new CommandLineBuilder()
                .SetContainer(container)
                .AddCommand(new CommandModel("exit", new Action(() => Environment.Exit(0))))
                .Build();
        }
    }
}
