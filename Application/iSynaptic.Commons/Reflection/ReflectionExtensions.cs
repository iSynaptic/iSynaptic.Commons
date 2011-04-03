using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using iSynaptic.Commons.Collections.Generic;

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

        public static MethodInfo FindConversionMethod(this Type onType, Type fromType, Type toType)
        {
            Guard.NotNull(onType, "onType");
            Guard.NotNull(fromType, "fromType");
            Guard.NotNull(toType, "toType");

            return onType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(x => new {Method = x, ToType = x.ReturnType, Parameters = x.GetParameters()})
                .Where(x => toType.IsAssignableFrom(x.ToType))
                .Where(x => x.Parameters.Length == 1)
                .Select(x => new {x.Method, FromType = x.Parameters[0].ParameterType, x.ToType})
                .Where(x => x.FromType.IsAssignableFrom(fromType))
                .OrderBy(x => x, (l, r) =>
                {
                    if (l.ToType == toType ^ r.ToType == toType)
                        return l.ToType == toType ? -1 : 1;

                    if (l.FromType == fromType ^ r.FromType == fromType)
                        return l.FromType == fromType ? -1 : 1;

                    if (l.Method.Name == "op_Implicit" ^ r.Method.Name == "op_Implicit")
                        return l.Method.Name == "op_Implicit" ? -1 : 1;

                    if (l.Method.Name == "op_Explicit" ^ r.Method.Name == "op_Explicit")
                        return l.Method.Name == "op_Explicit" ? -1 : 1;

                    return 0;
                })
                .Select(x => x.Method)
                .FirstOrDefault();
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
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var results = source
                .Flatten(x => Maybe.Value(x.BaseType).NotNull())
                .SelectMany(x => x.GetFields(bindingFlags).Where(y => y.DeclaringType == x));
            
            return filter != null
                ? results.Where(filter)
                : results;
        }
    }
}
