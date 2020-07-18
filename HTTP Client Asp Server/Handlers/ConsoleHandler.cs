using System;
using System.Collections.Generic;
using System.Linq;

public class ConsoleHandler
{
    public static List<CommandPair> Commands { get; set; } = new List<CommandPair> 
    { 
        new CommandPair("/Help", line=> Commands.ForEach(x => 
        {
            Console.WriteLine("Listing commands:");
            Console.WriteLine($"{x.Example}");
        })){Example="/Help"},
        new CommandPair("/Clear", line=> Console.Clear()){Example = "/Clear"},
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
