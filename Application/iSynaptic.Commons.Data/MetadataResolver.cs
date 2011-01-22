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

        public TMetadata Resolve<TMetadata, TSubject>(MetadataDeclaration<TMetadata> declaration, Maybe<TSubject> subject, MemberInfo member)
        {
            Guard.NotNull(declaration, "declaration");

            var subjectFunc = !subject.HasValue ? (Func<TSubject>)null : () => subject.Value;

            var request = new MetadataRequest<TMetadata, TSubject>(declaration, subjectFunc, member);

            var candidateBindings = _BindingSources
                .SelectMany(x => x.GetBindingsFor(request))
                .Where(x => x.Matches(request));
                
            var selectedBinding = SelectBinding(request, candidateBindings);

            if(selectedBinding == null)
                return declaration.Default;

            object scopeObject = selectedBinding.GetScopeObject(request);

            if (scopeObject != null)
            {
                ScopingCache<TMetadata>.Cache.PurgeGarbage();

                var cachedResult = ScopingCache<TMetadata>.Cache.TryGetValue(scopeObject);
                if(cachedResult.HasValue)
                    return cachedResult.Value;
            }

            var results = selectedBinding.Resolve(request);
            declaration.ValidateValue(results);

            if(scopeObject != null)
                ScopingCache<TMetadata>.Cache.Add(scopeObject, results);

            return results;
        }

        protected virtual IMetadataBinding<TMetadata, TSubject> SelectBinding<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request, IEnumerable<IMetadataBinding<TMetadata, TSubject>> candidates)
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
