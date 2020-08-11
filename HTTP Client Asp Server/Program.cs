using HTTP_Client_Asp_Server.Handlers;

namespace HTTP_Client_Asp_Server
{
    internal class Program
    {
        private static void Main()
        {
            ConsoleBuilder builder = new ConsoleBuilder("https://localhost:44391/api/");
            var console = builder.BuildConsole();
            console.Run();
        }
    }
}