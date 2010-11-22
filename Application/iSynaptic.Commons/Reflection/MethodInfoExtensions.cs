using System;
using System.Reflection;

namespace iSynaptic.Commons.Reflection
{
    internal static class MethodInfoExtensions
    {
        public static T ToDelegate<T>(this MethodInfo self)
        {
            if(self == null)
                throw new ArgumentNullException("self");

            return (T)(object)Delegate.CreateDelegate(typeof(T), self);

        }
    }
}
