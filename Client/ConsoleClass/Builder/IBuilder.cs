using Client.Models;
using StructureMap;
using System;

namespace Client.ConsoleClass
{
    public interface IBuilder
    {
        public IBuilder AddCommand(CommandModel command);

        public IBuilder SetContainer(Container container);

        public IBuilder Scan(Action<IAssemblyScanner> action);

        public CommandLineHandler Build();
    }
}