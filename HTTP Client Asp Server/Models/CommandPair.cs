using System;

public class CommandPair
{
    public CommandPair(string inputString, Action<string> operation)
    {
        InputString = inputString != null && inputString != "" ? inputString : throw new ArgumentNullException();
        Operation = operation;
    }

    public string InputString { get; set; }
    public string Example { get; set; }
    public string Description { get; set; }
    public Action<string> Operation { get; set; }
}