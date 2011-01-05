using System;
using System.Linq;
using System.Reflection;

namespace iSynaptic.Commons
{
    internal static class MethodReflectionObjectExtensions
    {
        public static T GetDelegate<T>(this object target, string methodName)
        {
            Guard.NotNull(target, "target");
            Guard.NotNullOrWhiteSpace(methodName, "methodName");

            Type delegateType = typeof (T);
            Type[] parameterTypes = delegateType.GetGenericArguments();
            
            if (delegateType.Name.StartsWith("Func"))
            {
                parameterTypes = parameterTypes
                    .Take(parameterTypes.Length - 1)
                    .ToArray();
            }

            Type targetType = target.GetType();

            MethodInfo info = targetType.GetMethod(methodName,
               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy,
               null,
               parameterTypes,
               null);

            if (info != null)
            {
                if(info.IsStatic)
                    return (T)(object)Delegate.CreateDelegate(typeof(T), info);

                return (T)(object)Delegate.CreateDelegate(typeof(T), target, info);
            }

            return default(T);
        }
    }
}
