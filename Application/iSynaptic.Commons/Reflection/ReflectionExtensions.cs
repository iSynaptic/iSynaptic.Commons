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

        public static IEnumerable<FieldInfo> GetFieldsDeeply(this Type source)
        {
            return GetFieldsDeeply(source, null);
        }

        public static IEnumerable<FieldInfo> GetFieldsDeeply(this Type source, Func<FieldInfo, bool> filter)
        {
            if(source == null)
                throw new ArgumentNullException("source");

            return GetFieldsDeeplyCore(source, filter);
        }

        private static IEnumerable<FieldInfo> GetFieldsDeeplyCore(this Type source, Func<FieldInfo, bool> filter)
        {
            Type currentType = source;
            while (currentType != null)
            {
                foreach (FieldInfo info in currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (filter != null && filter(info) != true)
                        continue;

                    yield return info;
                }

                if (currentType.BaseType != null)
                    currentType = currentType.BaseType;
                else
                    currentType = null;
            }
        }
    }
}
