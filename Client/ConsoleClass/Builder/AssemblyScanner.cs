using Client.Infrastructure;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Client.ConsoleClass;

internal class AssemblyScanner : IAssemblyScanner
{
    private Assembly assembly;
    private readonly List<Func<Type, bool>> classFilter;
    private readonly List<Func<MethodInfo, bool>> methodFilter;

    public AssemblyScanner()
    {
        assembly = Assembly.GetExecutingAssembly();
        classFilter = new List<Func<Type, bool>>()
        {
            c => c.IsClass,
        };
        methodFilter = new List<Func<MethodInfo, bool>>
        {
            m => m.HasAttribute<CommandAttribute>()
        };
    }

    public IAssemblyScanner AddClassFilter(Func<Type, bool> filter)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        classFilter.Add(filter);
        return this;
    }

    public IAssemblyScanner AddMethodFilter(Func<MethodInfo, bool> filter)
    {
        if (filter is null) throw new ArgumentNullException(nameof(filter));

        methodFilter.Add(filter);
        return this;
    }

    public IAssemblyScanner SetAssembly(Assembly assembly)
    {
        if (assembly is null) throw new ArgumentNullException(nameof(assembly));

        this.assembly = assembly;
        return this;
    }

    public IEnumerable<(Type type, IEnumerable<MethodInfo> methods)> ScanAssembly()
    {
        // Scan assembly for all classes that meed criteria,
        // then scan class for all methods that meet criteria,
        // remove classes with 0 methods.
        return assembly.GetExportedTypes()
            .ForAll(classFilter)
            .Select(c => (@class: c, methods: c.GetMethods().ForAll(methodFilter)))
            .Where(p => p.methods.Any());
    }
}
