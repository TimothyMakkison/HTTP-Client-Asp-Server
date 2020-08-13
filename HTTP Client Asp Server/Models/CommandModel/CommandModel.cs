using HTTP_Client_Asp_Server.Handlers;
using System;

public class CommandModel : ICommandData
{
    public CommandModel(ICommandData data)
    {
        CommandKey = data.CommandKey;
        Parsing = data.Parsing;
    }

    public CommandModel(string inputString, Action<string> operation)
    {
        CommandKey = inputString != null && inputString != "" ? inputString : throw new ArgumentNullException();
        Operation = operation;
    }

    public string CommandKey { get; set; }
    public Action<string> Operation { get; set; }
    public ParseMode Parsing { get; set; }
}