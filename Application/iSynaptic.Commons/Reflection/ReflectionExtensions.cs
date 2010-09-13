using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace iSynaptic.Commons.Reflection
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<T> GetAttributesOfType<T>(this ICustomAttributeProvider provider)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");

            return provider.GetCustomAttributes(true)
                .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
                .Cast<T>();
        }
    }
}
