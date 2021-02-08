using CSharpx;
using HTTP_Client_Asp_Server.Models;
using RailwaySharp;
using System.Collections.Generic;
using System.Linq;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public class CommandLineHandler
    {
        private IEnumerable<CommandModel> Commands { get; set; }

        public CommandLineHandler(string address)
        {
            Commands = new CommandLineBuilder(address).GetCommands();
        }

        public object Process(string input)
        {
            // If not a command then print "Not a command" else Parse input and run command or print error
            return Commands.SingleOrDefault(command => input.StartsWith(command.Data.CommandKey + ""))
                            .ToMaybe()
                            .Map(
                command => CommandParser.Parse(input,
                                               command).Either(
                    (parameters, _) => command.Operation.Invoke(parameters.ToArray()),
                    e => string.Join('\n', e)),
                () => "Not a command.");
        }
    }
}