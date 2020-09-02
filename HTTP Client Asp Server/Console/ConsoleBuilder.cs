﻿using HTTP_Client_Asp_Server.Models;
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

            var classCollection = new object[]
            {
                container.GetInstance<TalkBackHello>(),
                container.GetInstance<TalkBackSort>(),
                container.GetInstance<UserSender>(),
                container.GetInstance<ProtectedSender>(),
                container.GetInstance<ProtectedSignMessage>(),
                container.GetInstance<ProtectedAddFifty>(),
            };

            //TODO Search assembly and extract methods
            // Search assembly for valid methods/ classes, construct classes via container,
            // then extract valid funcs
            BaseFilter methodFilter = new CommandAttributeMethodFilter();
            var methods = methodFilter.GetValidMethods<Func<string, Task>>(classCollection);

            var commandModels = methods.Select(del 
                => new CommandModel(del.GetMethodInfo().GetCustomAttribute<CommandAttribute>(), del));
            return new ConsoleHandler(commandModels);
        }
    }
}