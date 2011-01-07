using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class MetadataResolver : IMetadataResolver
    {
        private static class ScopingCache<TMetadata>
        {
            public static readonly IWeakDictionary<object, TMetadata> Cache = new WeakKeyDictionary<object, TMetadata>();
        }

        private HashSet<IMetadataBindingSource> _BindingSources = new HashSet<IMetadataBindingSource>();

        public TMetadata Resolve<TMetadata>(MetadataDeclaration<TMetadata> declaration, object subject, MemberInfo member)
        {
            Guard.NotNull(declaration, "declaration");

            var request = new MetadataRequest<TMetadata>(declaration, subject, member);

            var candidateBindings = _BindingSources
                .SelectMany(x => x.GetBindingsFor(request))
                .Where(x => x.Matches(request));
                
            var selectedBinding = SelectBinding(request, candidateBindings);

            if(selectedBinding == null)
                return declaration.Default;

            var results = default(TMetadata);

            object scopeObject = null;

            if (selectedBinding.ScopeFactory != null)
            {
                scopeObject = selectedBinding.ScopeFactory(request);
                ScopingCache<TMetadata>.Cache.PurgeGarbage();
            }

            if(scopeObject != null && ScopingCache<TMetadata>.Cache.TryGetValue(scopeObject, out results))
                return results;
        
            results = selectedBinding.Resolve(request);
            declaration.ValidateValue(results);

            if(scopeObject != null)
                ScopingCache<TMetadata>.Cache.Add(scopeObject, results);

            return results;
        }

        protected virtual IMetadataBinding<TMetadata> SelectBinding<TMetadata>(MetadataRequest<TMetadata> request, IEnumerable<IMetadataBinding<TMetadata>> candidates)
        {
            Guard.NotNull(candidates, "candidates");

            try
            {
                return candidates.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("More than one metadata binding was found. Remove duplicate bindings or apply additional conditions to existing bindings to make them unambiguous.", ex);
            }
        }

        public void AddMetadataBindingSource<T>() where T : class, IMetadataBindingSource, new()
        {
            AddMetadataBindingSource(new T());
        }

        public void AddMetadataBindingSource(IMetadataBindingSource source)
        {
            Guard.NotNull(source, "source");

            _BindingSources.Add(source);
        }
    }
}
