using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public class CommandLineBuilder
    {
        private readonly HttpClient client;

        public CommandLineBuilder(string address)
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(address)
            };
        }

        public IEnumerable<CommandModel> GetCommands()
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
            var classInstances = valid.Select(container.GetInstance);

            var methods = classInstances.BuildValidMethods(m => m.HasAttribute<CommandAttribute>());

            return methods.Select(func => new CommandModel(func.GetMethodInfo().GetCustomAttribute<CommandAttribute>(), func));
        }
    }
}