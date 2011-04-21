using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{
    public class ExodataResolver : IExodataResolver
    {
        #region Cache Helper Classes

        private interface ICacheValue
        {
            int RequestHashCode { get; }
            TExodata GetExodata<TExodata>();
        }

        private class CacheValue<TExodata> : ICacheValue
        {
            public CacheValue(TExodata exodata, int requestHashCode)
            {
                Exodata = exodata;
                RequestHashCode = requestHashCode;
            }

            public TRequestedExodata GetExodata<TRequestedExodata>()
            {
                return Cast<TExodata, TRequestedExodata>.With(Exodata);
            }

            public TExodata Exodata { get; private set; }
            public int RequestHashCode { get; private set; }
        }

        #endregion

        private readonly MultiMap<object, ICacheValue> _Cache;
        private readonly WeakKeyDictionary<object, ICollection<ICacheValue>> _CacheDictionary;

        public ExodataResolver()
        {
            _CacheDictionary = new WeakKeyDictionary<object, ICollection<ICacheValue>>();
            _Cache = new MultiMap<object, ICacheValue>(_CacheDictionary);
        }

        private readonly HashSet<IExodataBindingSource> _BindingSources = new HashSet<IExodataBindingSource>();

        public Maybe<TExodata> Resolve<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
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
                _CacheDictionary.PurgeGarbage();

                var scopedCache = _Cache[scopeObject];
                var cachedValue = scopedCache.FirstOrDefault(x => x.RequestHashCode == requestHashCode);

                if (exodataScopeObject == null || exodataScopeObject.IsInScope<TExodata, TContext, TSubject>(selectedBinding, request))
                {
                    if (cachedValue != null)
                        return cachedValue.GetExodata<TExodata>();
                }
                else if(cachedValue != null)
                    _Cache.Remove(scopeObject, cachedValue);
            }

            var results = selectedBinding.Resolve<TExodata, TContext, TSubject>(request);

            if (scopeObject != null)
            {
                _Cache.Add(scopeObject, new CacheValue<TExodata>(results, requestHashCode));
                
                if (exodataScopeObject != null)
                {
                    exodataScopeObject.CacheFlushRequested += (s, a) => _CacheDictionary
                                                                    .TryGetValue(scopeObject)
                                                                    .Do(x => x.Clear())
                                                                    .Do(x => _CacheDictionary.Remove(scopeObject))
                                                                    .Run();
                }
            }

            return results;
        }

        protected virtual IExodataBinding SelectBinding<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request, IEnumerable<IExodataBinding> candidates)
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
