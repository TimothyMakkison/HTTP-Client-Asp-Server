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

    public List<CommandPair> Commands { get; set; } = new List<CommandPair>
    {
        new CommandPair("Exit", line=> Environment.Exit(0)),
    };

    public void ProcessLine(string line)
    {
        var matchingKeywords = Commands.Where(x => line.StartsWith(x.InputString));

        switch (matchingKeywords.Count())
        {
            case 1:
                matchingKeywords.FirstOrDefault().Operation.Invoke(line);
                break;

            case 0:
                Console.WriteLine("No matching commands please check spelling or type /Help");
                break;

            default:
                {
                    Console.WriteLine("Two or more commands match given input, please check commands for conflict");
                    break;
                }
        }
    }
}