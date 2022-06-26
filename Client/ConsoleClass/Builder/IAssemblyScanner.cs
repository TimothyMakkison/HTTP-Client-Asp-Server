using System;
using System.Collections.Generic;
using System.Reflection;

namespace Client.ConsoleClass;

public interface IAssemblyScanner
{
    IAssemblyScanner SetAssembly(Assembly assembly);

    IAssemblyScanner AddClassFilter(Func<Type, bool> filter);

    IAssemblyScanner AddMethodFilter(Func<MethodInfo, bool> filter);

    public IEnumerable<(Type type, IEnumerable<MethodInfo> methods)> ScanAssembly();
}
