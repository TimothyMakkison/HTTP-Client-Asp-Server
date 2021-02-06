using HTTP_Client_Asp_Server.Handlers;
using System;

namespace HTTP_Client_Asp_Server.Models.CommandModel
{
    public class CommandModel
    {
        public CommandModel(ICommandData commandData, Delegate operation)
        {
            Operation = operation;
            Data = commandData;
        }

        public ICommandData Data { get; private set; }
        public Delegate Operation { get; set; }
    }
}