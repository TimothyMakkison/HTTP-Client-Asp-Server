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
        private List<IAssemblyScanner> scanners = new List<IAssemblyScanner>();
        private Container container;

        public IBuilder AddCommand(CommandModel command)
        {
            commandModels.Add(command);
            return this;
        }

        public CommandLineHandler Build()
        {
            container ??= new Container();
            if(scanners.Count is 0)
            {
                Scan(_ => { });
            }

            // Scan instance and get all valid (Class Type, IEnumerable<MethodInfo>) pairs
            // Create (object, IEnumerable<MethodInfo>) pairs
            // Convert pairs into Delegates
            var a = scanners.SelectMany(s => s.ScanAssembly()).ToArray();
            var b = a.Select(c => (@class: container.GetInstance(c.type), c.methods))
                                .ToArray();
            var dels = b
                                .SelectMany(p => p.methods, (p, m) => m.CreateDelegate(p.@class)).ToArray();

            var commands = dels.Select(func => new CommandModel(func.GetMethodInfo().GetCustomAttribute<CommandAttribute>(), func));
            commandModels.AddRange(commands);

            return new CommandLineHandler(commandModels);
        }

        public IBuilder Scan(Action<IAssemblyScanner> action)
        {
            /*Scan - define assembly
            * define assembly filter
            * define class filter
            * define method filter
            * ie new Scan(x=>
            * x.Assembly(()=>Assembly.GetExecutingAssembly())
            * x.
            */

            var scanner = new AssemblyScanner();
            action(scanner);
            scanners.Add(scanner);
            return this;
        }

        public IBuilder SetContainer(Container container)
        {
            this.container = container;
            return this;
        }
    }
}