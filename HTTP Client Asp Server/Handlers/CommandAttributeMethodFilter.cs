using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HTTP_Client_Asp_Server.Handlers
{
    public class CommandAttributeMethodFilter : BaseFilter
    {
        public override IEnumerable<T> GetValidMethods<T>(IEnumerable<object> list) where T : class
        {
            // Get class instances and corresponding collection of valid methods
            IEnumerable<(object instance, IEnumerable<MethodInfo> methods)> validMethods = list.Where(x => x.GetType().IsClass)
                       .Select(instance => (instance, methods: instance.GetType()
                                                                       .GetMethods()
                                                                       .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0
                                                                       && IsMethodCompatibleWithDelegate<T>(m))));

            // Flatten into class instance, methodinfo pairing.
            var validMethodClassPair = validMethods.SelectMany(a => a.methods, (tuple, method) => (tuple.instance, method));
            // Convert into a func of type T.
            return validMethodClassPair.Select(x => x.method.CreateDelegate(typeof(T), x.instance) as T);
        }
    }
}