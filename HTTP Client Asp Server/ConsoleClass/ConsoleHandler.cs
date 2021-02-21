using RailwaySharp;
using System;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public class ConsoleHandler
    {
        public ConsoleHandler(CommandLineHandler handler)
        {
            Handler = handler;
        }

        private CommandLineHandler Handler { get; set; }

        public void Run()
        {
            Console.WriteLine("Hello. What would you like to do?");

            while (true)
            {
                var line = Console.ReadLine();
                Console.Clear();

                Result<object, string> output = Handler.Process(line);
                object result = output.Either((o, _) => o, e => string.Join(',', e));
                Console.WriteLine(result);
                Console.WriteLine("What would you like to do next ?");
            }
        }
    }
}