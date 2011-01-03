using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Reflection;

namespace iSynaptic.Commons.Data
{
    public class SurrogateMetadataBindingSource : IMetadataBindingSource
    {
        private static IDictionary<Type, object> _Surrogates = null;

        static SurrogateMetadataBindingSource()
        {
            Type bindingSourceType = typeof (IMetadataBindingSource);

            _Surrogates = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.IsDynamic != true)
                .SelectMany(x => x.GetExportedTypes())
                .Where(bindingSourceType.IsAssignableFrom)
                .Where(x => x.IsDefined(typeof(SurrogateMetadataBindingSourceAttribute), true))
                .Where(x => x.GetConstructors().Any(y => y.GetParameters().Length == 0))
                .SelectMany(InstantiateSurrogate)
                .ToReadOnlyDictionary();
        }

        private static IEnumerable<KeyValuePair<Type, object>> InstantiateSurrogate(Type type)
        {
            var surrogatesFor = type.GetAttributesOfType<SurrogateMetadataBindingSourceAttribute>();
            object surrogate = Activator.CreateInstance(type);

            foreach(var surrogateFor in surrogatesFor)
                yield return new KeyValuePair<Type, object>(surrogateFor.RealType, surrogate);
        }

        public IEnumerable<IMetadataBinding<TMetadata>> GetBindingsFor<TMetadata>(MetadataRequest<TMetadata> request)
        {
            if (request.Subject is Type != true)
                return Enumerable.Empty<IMetadataBinding<TMetadata>>();

            return _Surrogates
                .Where(x => x.Key.IsAssignableFrom((Type) request.Subject))
                .Select(x => x.Value)
                .Cast<IMetadataBindingSource>()
                .SelectMany(x => x.GetBindingsFor(request));
        }
    }
}
