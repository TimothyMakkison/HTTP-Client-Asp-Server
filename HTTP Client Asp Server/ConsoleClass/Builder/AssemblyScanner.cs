using HTTP_Client_Asp_Server.Infrastructure;
using HTTP_Client_Asp_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HTTP_Client_Asp_Server.ConsoleClass
{
    internal class AssemblyScanner : IAssemblyScanner
    {
        private Assembly assembly;
        private List<Func<Type, bool>> classFilter;
        private List<Func<MethodInfo, bool>> methodFilter;

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
            var a = assembly.GetExportedTypes()
                .ForAll(classFilter)
                .Select(c => (@class: c, c.GetMethods().ForAll(methodFilter))).ToArray();
            return a;
        }
    }
}