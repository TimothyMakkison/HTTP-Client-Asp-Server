using HTTP_Client_Asp_Server.Models;
using HTTP_Client_Asp_Server.Senders;
using StructureMap;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Handlers
{
    public class ConsoleBuilder
    {
        private HttpClient client;

        public ConsoleBuilder(string baseAddress)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        public ConsoleHandler BuildConsole()
        {
            var container = new Container(_ =>
            {
                _.ForSingletonOf<HttpClient>().Use(client);
                _.ForSingletonOf<UserHandler>();
                _.ForSingletonOf<CryptoKey>();
            });

            //Search assembly for classes that contain methods with chosen attribute
            var valid = Assembly.GetExecutingAssembly()
                                .GetExportedTypes()
                                .Where(x => x.IsClass)
                                .Where(inst => inst.GetMethods()
                                                   .Any(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0));

            //Take valid class types and create concrete instances.
            var classCollection = valid.Select(container.GetInstance);

            BaseFilter methodFilter = new CommandAttributeMethodFilter();
            var methods = methodFilter.GetValidMethods<Func<string, Task>>(classCollection);

            var commandModels = methods.Select(func => new CommandModel(func.GetMethodInfo().GetCustomAttribute<CommandAttribute>(), func));
            return new ConsoleHandler(commandModels);
        }
    }
}