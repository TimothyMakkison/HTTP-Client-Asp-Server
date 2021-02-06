using System;

namespace HTTP_Client_Asp_Server.Models
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