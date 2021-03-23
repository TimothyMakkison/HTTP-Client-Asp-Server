using System;

namespace Client.Models
{
    public class CommandModel
    {
        public CommandModel(string keyword, Delegate operation)
        {
            Data =new CommandData() { CommandKey = keyword };
            Operation = operation;
        }
        public CommandModel(ICommandData commandData, Delegate operation)
        {
            Operation = operation;
            Data = commandData;
        }

        public ICommandData Data { get; private set; }
        public Delegate Operation { get; set; }
    }
}