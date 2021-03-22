using HTTP_Client_Asp_Server.Infrastructure;
using RailwaySharp;
using System;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public class ConsoleHandler
    {
        private CommandLineHandler Handler { get; set; }
        private readonly I output;

        public ConsoleHandler(CommandLineHandler handler, I output)
        {
            Handler = handler;
            this.output = output;
        }

        public void Run()
        {
            //TODO Make programs write to some ILog or IPrint instead of directly to console.
            output.Print("Hello. What would you like to do?");

            while (true)
            {
                var line = Console.ReadLine();
                output.Clear();

                Result<object, string> hOutput = Handler.Process(line);
                object result = hOutput.Either((o, _) => o, e => string.Join(',', e));

                //TODO fix crash if returning void.
                output.Print(result.ToString());
                output.Print("What would you like to do next ?");
            }
        }
    }
}