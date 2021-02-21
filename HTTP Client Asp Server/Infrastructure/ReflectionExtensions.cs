using HTTP_Client_Asp_Server.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace HTTP_Client_Asp_Server.Infrastructure
{
    public static class ReflectionExtensions
    {
        //TODO Rework BuildMethod.
        public static IEnumerable<Delegate> BuildValidMethods(this IEnumerable<object> classInstances, Func<MethodInfo, bool> filter)
        {   
            //Filter for classes, then iterate through methods, selecting methods that take the same arguments and return type as T
            IEnumerable<(object instance, IEnumerable<MethodInfo> methods)> validMethods = classInstances.Where(x => x.GetType().IsClass)
                       .Select(instance => (instance, methods: instance.GetType()
                                                                       .GetMethods()
                                                                       .Where(filter)));

            // Flatten into class instance, methodinfo pairing.
            var validMethodClassPair = validMethods.SelectMany(pair => pair.methods,
                (tuple, method) => (tuple.instance, method));

            // Convert into a func of type T.
            return validMethodClassPair.Select(pair => pair.method.CreateDelegate(pair.instance));
        }

        public static bool HasAttribute<T>(this MethodInfo methodInfo) where T : Attribute
        {
            return methodInfo.GetCustomAttributes(typeof(T), false).Length > 0;
        }

        public static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var isAction = methodInfo.ReturnType.Equals(typeof(void));
            var types = methodInfo.GetParameters()
                                  .Select(p => p.ParameterType);

            if (isAction)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            return methodInfo.IsStatic
                ? Delegate.CreateDelegate(getType(types.ToArray()), methodInfo)
                : Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }

        public static TargetType ToTargetType(this Type type)
        {
            return type == typeof(bool)
                       ? TargetType.Switch
                       : type == typeof(string)
                             ? TargetType.Scalar
                             : type.IsArray || typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(type)
                                   ? TargetType.Sequence
                                   : TargetType.Scalar;
        }

        public static bool IsPrimitiveEx(this Type type)
        {
            return
                   (type.GetTypeInfo().IsValueType && type != typeof(Guid))
                || type.GetTypeInfo().IsPrimitive
                || new[] {
                     typeof(string)
                    ,typeof(decimal)
                    ,typeof(DateTime)
                    ,typeof(DateTimeOffset)
                    ,typeof(TimeSpan)
                   }.Contains(type)
                || Convert.GetTypeCode(type) != TypeCode.Object;
        }
    }
}