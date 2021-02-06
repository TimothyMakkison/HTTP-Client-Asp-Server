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

//TODO Separate input loop into a different method

//TODO Add parsing logic into command interpreter

//TODO Remove any additional info from command attribute aside from command word

//TODO Move all command construction and searching into separate class

//TODO Maybe make all return the http response?

//TODO Trim all keywords of spaces

//TODO Add check for bad parse

// TODO Console Runner (takes user input string and inputs into command handler) ->
//CommandHandler (Run command builder pass to command parser, then pass any strings to command parser) ->
//Commands Builder (Scan and construct commands) ->  Console Parser (Construct with commands, parse string input)
// -> Invoker (take string and method and attempt to pass arguments and invoke)