using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class SurrogateExodataBindingSource : IExodataBindingSource
    {
        private static Lazy<IDictionary<Type, object>> _Surrogates = new Lazy<IDictionary<Type, object>>(() =>
        {
            Type bindingSourceType = typeof(IExodataBindingSource);

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDynamic != true)
                .SelectMany(x => x.GetExportedTypes())
                .Where(bindingSourceType.IsAssignableFrom)
                .Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(ExodataSurrogate<>))
                .Where(x => x.GetConstructors().Any(y => y.GetParameters().Length == 0))
                .Select(InstantiateSurrogate)
                .ToReadOnlyDictionary();
        });

        private static KeyValuePair<Type, object> InstantiateSurrogate(Type type)
        {
            var surrogatesFor = type.BaseType.GetGenericArguments()[0];
            object surrogate = Activator.CreateInstance(type);

            return new KeyValuePair<Type, object>(surrogatesFor, surrogate);
        }

        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TSubject>(IExodataRequest<TSubject> request)
        {
            Type subjectType = typeof(TSubject);

            return _Surrogates.Value
                .Where(x => x.Key.IsAssignableFrom(subjectType))
                .Select(x => x.Value)
                .Cast<IExodataBindingSource>()
                .SelectMany(x => x.GetBindingsFor<TExodata, TSubject>(request));
        }
    }
}
