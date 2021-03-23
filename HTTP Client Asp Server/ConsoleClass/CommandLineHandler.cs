using CSharpx;
using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using RailwaySharp;
using System.Collections.Generic;
using System.Linq;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public class CommandLineHandler
    {
        private IEnumerable<CommandModel> Commands { get; set; }

        public CommandLineHandler(IEnumerable<CommandModel> commands)
        {
            Commands = commands;
            Commands.ToList().ForEach(x => System.Console.WriteLine(x.Data.CommandKey));
        }

        public Result<object,string> Process(string input)
        {
            // Get command parse arguments into object array and then invoke.
            // Return output.
            var command = GetCommand(input);
            return command.Bind(command => CommandParser.Parse(input, command))
                .Bind(arguments => command.Map(c => c.Operation.Invoke(arguments.ToArray())));
        }
        private Result<CommandModel,string> GetCommand(string input)
        {
            // Find matching command either returning the value or error message.
            return Commands.SingleOrDefault(command => input.StartsWith(command.Data.CommandKey + ""))
                            .ToMaybe()
                            .Map(command => Result<CommandModel, string>.Succeed(command),
                            () => Result<CommandModel, string>.FailWith("Not a command."));


        }
    }
}