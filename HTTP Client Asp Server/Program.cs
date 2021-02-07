using HTTP_Client_Asp_Server.ConsoleClass;

namespace HTTP_Client_Asp_Server
{
    internal class Program
    {
        private static void Main()
        {
            string address = @"https://localhost:44391/api/";
            var console = new ConsoleHandler(new CommandLineHandler(address));
            console.Run();
        }
    }
}

//TODO Maybe make all return the http response?

