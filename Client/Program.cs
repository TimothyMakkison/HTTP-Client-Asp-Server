﻿using Client.ConsoleClass;
using Client.Infrastructure;
using Client.Models;
using Client.Senders;
using StructureMap;
using System;
using System.Net.Http;

namespace Client;

internal class Program
{
    private static void Main()
    {
        const string address = @"https://localhost:44391/api/";
        var consoleOutput = new ConsoleOutput();

        CommandLineHandler handler = BuildHandler(address, consoleOutput);

        var console = new ConsoleHandler(handler, consoleOutput);
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
