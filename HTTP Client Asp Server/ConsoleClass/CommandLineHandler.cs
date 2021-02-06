using CSharpx;
using HTTP_Client_Asp_Server.Models;
using RailwaySharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    (parameters, _) => (object)Invoke(command.Operation, parameters.ToArray()),
                    e => string.Join('\n', e)),
                () => "Not a command.");
        }

        private static string Invoke(Delegate delgate, object[] parameters)
        {
            // Invoke command and wait if it returns a task value.
            var returnValue = delgate.DynamicInvoke(parameters);
            if (returnValue is Task) (returnValue as Task).Wait();

            return "";
        }
    }
}