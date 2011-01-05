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
            Guard.NotNull(provider, "provider");

            Type desiredType = typeof (T);

            return provider.GetCustomAttributes(true)
                .Where(x => desiredType.IsAssignableFrom(x.GetType()))
                .Cast<T>();
        }

        public static IEnumerable<FieldInfo> GetFieldsDeeply(this Type source)
        {
            return GetFieldsDeeply(source, null);
        }

        public static IEnumerable<FieldInfo> GetFieldsDeeply(this Type source, Func<FieldInfo, bool> filter)
        {
            Guard.NotNull(source, "source");
            return GetFieldsDeeplyCore(source, filter);
        }

        private static IEnumerable<FieldInfo> GetFieldsDeeplyCore(this Type source, Func<FieldInfo, bool> filter)
        {
            Type currentType = source;
            while (currentType != null)
            {
                foreach (var fieldInfo in currentType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (filter != null && filter(fieldInfo) != true)
                        continue;

                    yield return fieldInfo;
                }

                if (currentType.BaseType != null)
                    currentType = currentType.BaseType;
                else
                    currentType = null;
            }
        }
    }
}
