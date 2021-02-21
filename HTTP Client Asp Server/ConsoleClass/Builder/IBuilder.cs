using HTTP_Client_Asp_Server.Models;
using StructureMap;
using System;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public interface IBuilder
    {
        public IBuilder AddCommand(CommandModel command);

        public IBuilder SetContainer(Container container);

        public IBuilder Scan(Action<IAssemblyScanner> action);

        public CommandLineHandler Build();
    }
}