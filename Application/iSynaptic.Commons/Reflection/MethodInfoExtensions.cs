using System;
using System.Reflection;

namespace iSynaptic.Commons.Reflection
{
    internal static class MethodInfoExtensions
    {
        public static T ToDelegate<T>(this MethodInfo @this)
        {
            Guard.NotNull(@this, "@this");
            return (T)(object)Delegate.CreateDelegate(typeof(T), @this);
        }
    }
}
