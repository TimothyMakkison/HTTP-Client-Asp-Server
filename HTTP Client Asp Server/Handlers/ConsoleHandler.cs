using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class ConsoleHandler
{
    public static bool Running { get; set; }
    public void Run()
    {
        Running = true;
        Console.WriteLine("Hello. What would you like to do?");

        while (Running)
        {
            var line = Console.ReadLine();
            Console.Clear();
            ProcessLine(line);
            Console.WriteLine("What would you like to do next ?");
        }
    }
    public List<CommandPair> Commands { get; set; } = new List<CommandPair> 
    { 
        //new CommandPair("/Help", line=> Commands.ForEach(x => 
        //{
        //    Console.WriteLine("Listing commands:");
        //    Console.WriteLine($"{x.Example}");
        //})){Example="/Help"},
        new CommandPair("Exit", line=> Running=false){Example = "/Clear"},
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
                    foreach (var matching in matchingKeywords)
                    {
                        Console.WriteLine(matching.Example);
                    }
                    break;
                }
        }
    }
}
