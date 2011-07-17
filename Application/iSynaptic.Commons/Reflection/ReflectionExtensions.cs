using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Linq;

namespace iSynaptic.Commons.Reflection
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<T> GetAttributesOfType<T>(this ICustomAttributeProvider provider)
        {
            return GetAttributesOfType(provider, typeof(T))
                .Cast<T>();
        }

        public static IEnumerable<Attribute> GetAttributesOfType(this ICustomAttributeProvider provider, Type attributeType)
        {
            Guard.NotNull(provider, "provider");
            Guard.NotNull(attributeType, "attributeType");

            return provider.GetCustomAttributes(true)
                .Where(x => x.GetType().DoesImplementType(attributeType))
                .Cast<Attribute>(); 
        }

        public static bool DoesImplementType(this Type candidateType, Type testType)
        {
            Guard.NotNull(candidateType, "candidateType");
            Guard.NotNull(testType, "testType");

            if (!testType.IsGenericTypeDefinition)
                return testType.IsAssignableFrom(candidateType);

            return candidateType.Recurse(x => Maybe.NotNull(x.BaseType))
                .Select(TryGetGenericTypeDefinition)
                .Union(candidateType.GetInterfaces().Select(TryGetGenericTypeDefinition))
                .Where(x => x.HasValue)
                .Any(x => x.Value == testType);
        }

        public static Maybe<Type> TryGetGenericTypeDefinition(this Type source)
        {
            return Maybe.If(source.IsGenericTypeDefinition, source.ToMaybe())
                .Or(() => Maybe.If(source.IsGenericType, source.GetGenericTypeDefinition().ToMaybe()));
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
                .OrderByPriorities(x => x.ToType == toType,
                                   x => x.FromType == fromType,
                                   x => x.Method.Name == "op_Implicit",
                                   x => x.Method.Name == "op_Explicit")
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
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var results = source
                .Recurse(x => Maybe.NotNull(x.BaseType))
                .SelectMany(x => x.GetFields(bindingFlags).Where(y => y.DeclaringType == x));
            
            return filter != null
                ? results.Where(filter)
                : results;
        }
    }
}
