using HTTP_Client_Asp_Server.ConsoleClass;

namespace HTTP_Client_Asp_Server
{
    internal class Program
    {
        private static void Main()
        {
            string address = @"https://localhost:44391/api/";
<<<<<<< Updated upstream
            var console = new ConsoleHandler(new CommandLineHandler(address));
=======
            var client = new HttpClient
            {
                BaseAddress = new Uri(address)
            };

            var container = new Container(_ =>
            {
                _.ForSingletonOf<HttpClient>().Use(client);
                _.ForSingletonOf<UserHandler>();
                _.ForSingletonOf<CryptoKey>();
            });

            var handler = new CommandLineBuilder()
                .SetContainer(container)
                .AddCommand(new CommandModel("exit", new Action(() => Environment.Exit(0))))
                .Build();

            var console = new ConsoleHandler(handler);
>>>>>>> Stashed changes
            console.Run();
        }
    }
}

//TODO Maybe make all return the http response?