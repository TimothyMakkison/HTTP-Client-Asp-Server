﻿using HTTP_Client_Asp_Server.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

public class ConsoleHandler
{
    public void Run()
    {
        Console.WriteLine("Hello. What would you like to do?");

        while (true)
        {
            var line = Console.ReadLine();
            Console.Clear();
            ProcessLine(line);
            Console.WriteLine("What would you like to do next ?");
        }
    }

    private List<CommandModel> commands { get; set; } = new List<CommandModel>
    {
        new CommandModel("Exit", line=> Environment.Exit(0)),
    };

    public ConsoleHandler AddRange(IEnumerable<CommandModel> commandPairs)
    {
        commands.AddRange(commandPairs);
        return this;
    }

    public void ProcessLine(string line)
    {
        var matchingKeywords = commands.Where(x => line.StartsWith(x.CommandKey));

        switch (matchingKeywords.Count())
        {
            case 1:
                CommandInvoker.Invoke(matchingKeywords.FirstOrDefault(), line);
                break;

            case 0:
                Console.WriteLine("No matching commands please check spelling or type /Help");
                break;

            default:
                Console.WriteLine("Two or more commands match given input, please check commands for conflict");
                break;
        }
    }
}