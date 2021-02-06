using CSharpx;
using HTTP_Client_Asp_Server.Models.CommandModel;
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
            //var mc = Commands.SingleOrDefault(command => input.StartsWith(command.Data.CommandKey)).ToMaybe();
            //if (mc.IsNothing())
            //{
            //    return "Not a command.";
            //}
            //var matchingCommand = mc.FromJust();

            //return mc.Map(cm => cm.ToString(), () => "Not a command.");




            //var command = Commands.SingleOrDefault(command => input.StartsWith(command.Data.CommandKey));

            //var nums = new int[] { 14, 2, 133 };
            //var str = "word";

            //var ob = new object[] { nums, str };

            //return Invoke(command.Operation, ob);


            return Commands.SingleOrDefault(command => input.StartsWith(command.Data.CommandKey+""))
                            .ToMaybe()
                            .Map(
                command => CommandParser.Parse(input,
                                               command).Either(
                    (parameters, _) => (object)Invoke(command.Operation, parameters.ToArray()),
                    e => string.Join('\n', e)),
                () => "Not a command.");




            //var parseResult = CommandParser.Parse(input, matchingCommand);
            //var a = parseResult.Either((parameters, _) => (object)Invoke(matchingCommand.Operation, parameters)
            //, e => string.Join('\n', e));

            //var a = CommandParser.Parse(input, matchingCommand).Either(
            //    (parameters, _) => (object)Invoke(matchingCommand.Operation, parameters), e => string.Join('\n', e));

            //return Trial.Failed(CommandParser.Parse(input, matchingCommand))
            //    ? CommandParser.Parse(input, matchingCommand).FailedWith()
            //    : (object)Invoke(matchingCommand.Operation, CommandParser.Parse(input, matchingCommand).SucceededWith());
            //return objects.Match((ob, e) => Invoke(matchingCommand.Operation, objects.SucceededWith()), err => string.Join(',', err));

            //Invoke(matchingCommand.Operation, objects.SucceededWith());
            //Task task = objects.SucceededWith().Count() == 0
            //    ? (Task)matchingCommand.Operation.DynamicInvoke()
            //    : (Task)matchingCommand.Operation.DynamicInvoke(objects);
            //task.Wait();

            //return "";
        }

        private static string Invoke(Delegate delgate, object[] parameters)
        {
            Task task = parameters != null
               ? (Task)delgate.DynamicInvoke(parameters)
               : (Task)delgate.DynamicInvoke();
            task.Wait();

            return "";
        }
    }
}