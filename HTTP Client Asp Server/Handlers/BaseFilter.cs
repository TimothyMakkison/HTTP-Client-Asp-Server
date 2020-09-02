using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HTTP_Client_Asp_Server.Handlers
{
    public abstract class BaseFilter
    {
        public Func<MethodInfo, bool> Filter { get; set; }

        public abstract IEnumerable<T> GetValidMethods<T>(IEnumerable<object> list) where T : class;

        /// <summary>
        /// Compares a method info and Delegate returning true if they match.
        /// </summary>
        /// <typeparam name="T">Delegate type.</typeparam>
        /// <param name="methodInfo">MethodInfo</param>
        /// <returns>Bool determining if the T and methodInfo match.</returns>
        protected bool IsMethodCompatibleWithDelegate<T>(MethodInfo methodInfo) where T : class
        {
            Type delegateType = typeof(T);
            MethodInfo delegateSignature = delegateType.GetMethod("Invoke");

            bool parametersEqual = delegateSignature
                .GetParameters()
                .Select(x => x.ParameterType)
                .SequenceEqual(methodInfo.GetParameters()
                    .Select(x => x.ParameterType));
            bool returnEqual = delegateSignature.ReturnType == methodInfo.ReturnType;

            return returnEqual && parametersEqual;
        }
    }
}