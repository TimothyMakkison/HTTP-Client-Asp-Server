using CSharpx;
using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    public class CommandLineBuilder : IBuilder
    {
        public List<CommandModel> commandModels = new List<CommandModel>();
        private Container container;

        public IBuilder AddCommand(CommandModel command)
        {
            commandModels.Add(command);
            return this;
        }

        public CommandLineHandler Build()
        {
            container ??= new Container();

            //Search assembly for classes that contain methods with chosen attribute
            var valid = Assembly.GetExecutingAssembly()
                                .GetExportedTypes()
                                .Where(inst => inst.IsClass
                                && inst.GetMethods()
                                .Any(m => m.HasAttribute<CommandAttribute>()));

            //Take valid class types and create concrete instances.
            var classInstances = valid.Select(container.TryGetInstance);

            var methods = classInstances.BuildValidMethods(m => m.HasAttribute<CommandAttribute>());

            var commands = methods.Select(func => new CommandModel(func.GetMethodInfo().GetCustomAttribute<CommandAttribute>(), func));
            commandModels.AddRange(commands);

            return new CommandLineHandler(commandModels);
        }

        /*Add commands ie exit
         *  Scan assembly
         *  Add containers
         *  Build - return CommandLineHand
         */

        public IBuilder Scan()
        {
            throw new NotImplementedException();
        }

        public IBuilder SetContainer(Container container)
        {
            this.container = container;
            return this;
        }
    }
}