using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HTTP_Client_Asp_Server.Handlers
{
    public class MethodFilter
    {
        public Func<MethodInfo, bool> Filter { get; set; }

        public IEnumerable<(Delegate, MethodInfo)> GetValidMethods<T>(IEnumerable<object> list) where T : class
        {
            var validMethods = list.Where(x => x.GetType().IsClass)
                       .Select(instance => (instance, methods: instance.GetType()
                                                                       .GetMethods()
                                                                       .Where(m => Filter.Invoke(m)
                                                                       && IsMethodCompatibleWithDelegate<T>(m))));

            //var validMethodClassPair = validMethods.SelectMany(a => a.methods, (tuple, method) => (tuple.instance, method));

            //return validMethodClassPair.Select(x =>
            //           new CommandModel(x.method.GetCustomAttribute<CommandAttribute>())
            //           { Operation = (Action<string>)x.method.CreateDelegate(typeof(Action<string>), x.instance) });
            var validMethodClassPair = validMethods.SelectMany(a => a.methods, (tuple, method) => (tuple.instance, method));

            return validMethodClassPair.Select(x => (x.method.CreateDelegate(typeof(Action<string>), x.instance), x.method));
        }

        public IEnumerable<CommandModel> ToCommandModel(IEnumerable<(Delegate del, MethodInfo info)> pairs)
        {
            return pairs.Select(x => new CommandModel(x.info.GetCustomAttribute<CommandAttribute>()) { Operation = (Action<string>)x.del });
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