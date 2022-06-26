using CSharpx;
using Client.Infrastructure;
using Client.Models;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Client.ConsoleClass;

public class CommandLineBuilder : IBuilder
{
    public List<CommandModel> commandModels = new List<CommandModel>();
    private readonly List<IAssemblyScanner> scanners = new List<IAssemblyScanner>();
    private Container container;

    public IBuilder AddCommand(CommandModel command)
    {
        commandModels.Add(command);
        return this;
    }

    public IBuilder Scan(Action<IAssemblyScanner> action)
    {
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

    public CommandLineHandler Build()
    {
        container ??= new Container();
        if (scanners.Count is 0)
        {
            Scan(_ => { });
        }

        RunScanners();

        return new CommandLineHandler(commandModels);
    }

    private void RunScanners()
    {
        // Scan instance and get all valid (Class Type, IEnumerable<MethodInfo>) pairs
        // Create IEnumerable<(object, MethodInfo)> pairs
        // Convert pairs into Delegates
        var delegateCollection = scanners.SelectMany(s => s.ScanAssembly())
                           .SelectMany(p => p.methods,
                           (p, method) => (@class: container.GetInstance(p.type), method))
                           .Select(p => p.method.CreateDelegate(p.@class));

        var commands = delegateCollection.Select(func => new CommandModel(func.GetMethodInfo()
                                                                .GetCustomAttribute<CommandAttribute>(), func));
        commandModels.AddRange(commands);
    }

}
