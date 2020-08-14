using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HTTP_Client_Asp_Server.Handlers
{
    public class MethodFilter
    {
        public Func<MethodInfo, bool> Filter { get; set; }

        public IEnumerable<T> GetValidMethods<T>(IEnumerable<object> list) where T : class
        {
            var validMethods = list.Where(x => x.GetType().IsClass)
                       .Select(instance => (instance, methods: instance.GetType()
                                                                       .GetMethods()
                                                                       .Where(m => Filter.Invoke(m)
                                                                       && IsMethodCompatibleWithDelegate<T>(m))));

            var validMethodClassPair = validMethods.SelectMany(a => a.methods, (tuple, method) => (tuple.instance, method));
            return validMethodClassPair.Select(x => x.method.CreateDelegate(typeof(T), x.instance) as T);
        }

        public IEnumerable<CommandModel> ToCommandModel(IEnumerable<Func<string, Task>> delegates)
        {
            return delegates.Select(del => new CommandModel(del.GetMethodInfo().GetCustomAttribute<CommandAttribute>())
            {
                Operation = del
            });
        }

        public bool IsMethodCompatibleWithDelegate<T>(MethodInfo method) where T : class
        {
            Type delegateType = typeof(T);
            MethodInfo delegateSignature = delegateType.GetMethod("Invoke");

            bool parametersEqual = delegateSignature
                .GetParameters()
                .Select(x => x.ParameterType)
                .SequenceEqual(method.GetParameters()
                    .Select(x => x.ParameterType));
            bool returnEqual = delegateSignature.ReturnType == method.ReturnType;

            return returnEqual && parametersEqual;
        }
    }
}