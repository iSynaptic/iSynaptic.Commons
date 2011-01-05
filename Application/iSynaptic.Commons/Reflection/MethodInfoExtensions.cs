using System;
using System.Reflection;

namespace iSynaptic.Commons.Reflection
{
    internal static class MethodInfoExtensions
    {
        public static T ToDelegate<T>(this MethodInfo self)
        {
            Guard.NotNull(self, "self");
            return (T)(object)Delegate.CreateDelegate(typeof(T), self);
        }
    }
}
