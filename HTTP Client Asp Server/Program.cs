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
//TODO Separate input loop into a different method

//TODO Add parsing logic into command interpreter

//TODO Remove any additional info from command attribute aside from command word

//TODO Move all command construction and searching into separate class