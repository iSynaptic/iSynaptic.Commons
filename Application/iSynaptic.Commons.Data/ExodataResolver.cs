using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class ExodataResolver : IExodataResolver
    {
        private class CacheValue<TExodata>
        {
            public int RequestHashCode;
            public TExodata Exodata;
        }

        private static class ScopedCache<TExodata>
        {
            public static readonly MultiMap<object, CacheValue<TExodata>> Cache;
            public static readonly WeakKeyDictionary<object, ICollection<CacheValue<TExodata>>> Dictionary;

            static ScopedCache()
            {
                Dictionary = new WeakKeyDictionary<object, ICollection<CacheValue<TExodata>>>();
                Cache = new MultiMap<object, CacheValue<TExodata>>(Dictionary);
            }
        }

        private readonly HashSet<IExodataBindingSource> _BindingSources = new HashSet<IExodataBindingSource>();

        public Maybe<TExodata> Resolve<TExodata, TContext, TSubject>(IExodataRequest<TContext, TSubject> request)
        {
            Guard.NotNull(request, "request");

            int requestHashCode = request.GetHashCode();

            var candidateBindings = _BindingSources
                .SelectMany(x => x.GetBindingsFor<TExodata, TContext, TSubject>(request))
                .Where(x => x.Matches<TExodata, TContext, TSubject>(request));

            var selectedBinding = SelectBinding<TExodata, TContext, TSubject>(request, candidateBindings);

            if(selectedBinding == null)
                return Maybe<TExodata>.NoValue;

            object scopeObject = selectedBinding.GetScopeObject<TExodata, TContext, TSubject>(request);
            var exodataScopeObject = scopeObject as IExodataScopeObject;

            if (scopeObject != null)
            {
                ScopedCache<TExodata>.Dictionary.PurgeGarbage();

                if (exodataScopeObject == null || exodataScopeObject.IsInScope<TExodata, TContext, TSubject>(selectedBinding, request))
                {
                    var scopedCache = ScopedCache<TExodata>.Cache[scopeObject];
                    var cachedValue = scopedCache.FirstOrDefault(x => x.RequestHashCode == requestHashCode);

                    if (cachedValue != null)
                        return cachedValue.Exodata;
                }
            }

            var results = selectedBinding.Resolve<TExodata, TContext, TSubject>(request);

            if (scopeObject != null)
            {
                ScopedCache<TExodata>.Cache.Add(scopeObject, new CacheValue<TExodata> { Exodata = results, RequestHashCode = requestHashCode });
                
                if (exodataScopeObject != null)
                {
                    exodataScopeObject.ScopeClosed += (s, a) => ScopedCache<TExodata>.Dictionary
                                                                    .TryGetValue(scopeObject)
                                                                    .Do(x => x.Clear())
                                                                    .Do(x => ScopedCache<TExodata>.Dictionary.Remove(scopeObject))
                                                                    .Run();
                }
            }

            return results;
        }

        protected virtual IExodataBinding SelectBinding<TExodata, TContext, TSubject>(IExodataRequest<TContext, TSubject> request, IEnumerable<IExodataBinding> candidates)
        {
            Guard.NotNull(candidates, "candidates");

            try
            {
                return candidates.SingleOrDefault();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("More than one Exodata binding was found. Remove duplicate bindings or apply additional conditions to existing bindings to make them unambiguous.", ex);
            }
        }

        public void AddExodataBindingSource<T>() where T : IExodataBindingSource, new()
        {
            AddExodataBindingSource(new T());
        }

        public void AddExodataBindingSource(IExodataBindingSource source)
        {
            Guard.NotNull(source, "source");

            _BindingSources.Add(source);
        }

        public void RemoveExodataBindingSource<T>() where T : IExodataBindingSource
        {
            _BindingSources.RemoveAll(x => x is T);
        }

        public void RemoveExodataBindingSource(IExodataBindingSource source)
        {
            Guard.NotNull(source, "source");
            _BindingSources.Remove(source);
        }
    }
}
