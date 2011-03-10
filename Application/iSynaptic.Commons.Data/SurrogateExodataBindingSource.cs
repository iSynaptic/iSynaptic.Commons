using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class SurrogateExodataBindingSource : IExodataBindingSource
    {
        private Lazy<IDictionary<Type, object>> _Surrogates = new Lazy<IDictionary<Type, object>>(() =>
        {
            Type bindingSourceType = typeof(IExodataBindingSource);

            return AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDynamic != true)
                .SelectMany(x => x.GetExportedTypes())
                .Where(bindingSourceType.IsAssignableFrom)
                .Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(ExodataSurrogate<>))
                .Select(InstantiateSurrogate)
                .ToReadOnlyDictionary();
        });

        private static KeyValuePair<Type, object> InstantiateSurrogate(Type type)
        {
            var surrogatesFor = type.BaseType.GetGenericArguments()[0];
            object surrogate = Ioc.Resolve(type);
            
            if(surrogate == null && type.GetConstructors().Any(x => x.GetParameters().Length == 0))
                surrogate = Activator.CreateInstance(type);

            if (surrogate == null)
                throw new InvalidOperationException(string.Format("Unable to instantiate Exodata surrogate '{0}'.", type.FullName));

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
