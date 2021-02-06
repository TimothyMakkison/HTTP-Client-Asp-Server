using HTTP_Client_Asp_Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HTTP_Client_Asp_Server.Handlers
{
    public static class CommandAttributeMethodFilter 
    {
        public static IEnumerable<Delegate> BuildValidMethods(this IEnumerable<object> list) 
        {
            //Filter for classes, then iterate through methods, selecting methods that take the same arguments and return type as T
            IEnumerable<(object instance, IEnumerable<MethodInfo> methods)> validMethods = list.Where(x => x.GetType().IsClass)
                       .Select(instance => (instance, methods: instance.GetType()
                                                                       .GetMethods()
                                                                       .Where(m => m.HasAttribute<CommandAttribute>())));

            // Flatten into class instance, methodinfo pairing.
            var validMethodClassPair = validMethods.SelectMany(a => a.methods, (tuple, method) => (tuple.instance, method));

            // Convert into a func of type T.
            return validMethodClassPair.Select(x => x.method.CreateDelegate(x.instance));
        }
    }
}