using System;

namespace HTTP_Client_Asp_Server.Models
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute, ICommandData
    {
        public string CommandKey { get; }

        public CommandAttribute(string commandKey)
        {
            if (commandKey == null || commandKey == "")
                throw new ArgumentNullException(nameof(commandKey));

            CommandKey = commandKey;
        }
    }
}