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

            var info = Maybe.NotNull(() => targetType.GetMethod(methodName,
               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy,
               null,
               parameterTypes,
               null));

            return info
                .Where(x => x.IsStatic)
                .Select(x => Delegate.CreateDelegate(typeof (T), x))
                .Or(info.Select(x => Delegate.CreateDelegate(typeof (T), target, x)))
                .Cast<T>()
                .Return();
        }
    }
}
