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
        private class CacheValue<TMetadata>
        {
            public int RequestHashCode;
            public TMetadata Metadata;
        }

        private static class ScopedCache<TMetadata>
        {
            public static readonly MultiMap<object, CacheValue<TMetadata>> Cache;
            public static readonly WeakKeyDictionary<object, ICollection<CacheValue<TMetadata>>> Dictionary;

            static ScopedCache()
            {
                Dictionary = new WeakKeyDictionary<object, ICollection<CacheValue<TMetadata>>>();
                Cache = new MultiMap<object, CacheValue<TMetadata>>(Dictionary);
            }
        }

        private readonly HashSet<IMetadataBindingSource> _BindingSources = new HashSet<IMetadataBindingSource>();

        public Maybe<TMetadata> Resolve<TMetadata, TSubject>(IMetadataDeclaration declaration, Maybe<TSubject> subject, MemberInfo member)
        {
            Guard.NotNull(declaration, "declaration");

            var request = new MetadataRequest<TMetadata, TSubject>(declaration, subject, member);
            int requestHashCode = request.GetHashCode();

            var candidateBindings = _BindingSources
                .SelectMany(x => x.GetBindingsFor(request))
                .Where(x => x.Matches(request));
                
            var selectedBinding = SelectBinding(request, candidateBindings);

            if(selectedBinding == null)
                return Maybe<TMetadata>.NoValue;

            object scopeObject = selectedBinding.GetScopeObject(request);

            if (scopeObject != null)
            {
                ScopedCache<TMetadata>.Dictionary.PurgeGarbage();

                var scopedCache = ScopedCache<TMetadata>.Cache[scopeObject];
                var cachedValue = scopedCache.FirstOrDefault(x => x.RequestHashCode == requestHashCode);

                if(cachedValue != null)
                    return cachedValue.Metadata;
            }

            var results = selectedBinding.Resolve(request);

            if(scopeObject != null)
                ScopedCache<TMetadata>.Cache.Add(scopeObject, new CacheValue<TMetadata> { Metadata = results, RequestHashCode = requestHashCode });

            return results;
        }

        protected virtual IMetadataBinding SelectBinding<TMetadata, TSubject>(IMetadataRequest<TMetadata, TSubject> request, IEnumerable<IMetadataBinding> candidates)
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
