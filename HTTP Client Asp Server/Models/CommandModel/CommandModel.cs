using HTTP_Client_Asp_Server.Handlers;
using System;
using System.Threading.Tasks;

public class CommandModel : ICommandData
{
    public CommandModel(ICommandData data)
    {
        CommandKey = data.CommandKey;
        Parsing = data.Parsing;
    }

    public CommandModel(string inputString, Func<string, Task> operation)
    {
        CommandKey = inputString != null && inputString != "" ? inputString : throw new ArgumentNullException();
        Operation = operation;
    }

    public string CommandKey { get; set; }
    public Func<string, Task> Operation { get; set; }
    public ParseMode Parsing { get; set; }
}