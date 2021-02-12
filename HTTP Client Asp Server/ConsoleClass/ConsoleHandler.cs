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

                var output = Handler.Process(line);
                Console.WriteLine(output.Either((o, _) => o, e => string.Join(',', e)));
                Console.WriteLine("What would you like to do next ?");
            }
        }
    }
}