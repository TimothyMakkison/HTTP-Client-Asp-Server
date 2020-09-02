using HTTP_Client_Asp_Server.Handlers;
using System;
using System.Threading.Tasks;

public class CommandModel
{
    public CommandModel(ICommandData commandData, Func<string, Task> operation)
    {
        Operation = operation;
        Data = commandData;
    }
    public ICommandData Data { get; private set; }
    public Func<string, Task> Operation { get; set; }
}