using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.Data
{

    public static class ExodataBinding
    {
        public static IExodataBinding Create<TExodata, TContext, TSubject>(IExodataBindingSource source, Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate, Func<IExodataRequest<TExodata, TContext, TSubject>, TExodata> valueFactory, Func<IExodataRequest<TExodata, TContext, TSubject>, object> scopeFactory = null, bool boundToContextInstance = false, bool boundToSubjectInstance = false)
        {
            Guard.NotNull(predicate, "predicate");
            Guard.NotNull(valueFactory, "valueFactory");
            Guard.NotNull(source, "source");

            return new TypedBinding<TExodata, TContext, TSubject>(predicate, valueFactory, source)
            {
                ContextType = typeof(TContext),
                SubjectType = typeof(TSubject),
                ScopeFactory = scopeFactory,
                BoundToContextInstance = boundToContextInstance,
                BoundToSubjectInstance = boundToSubjectInstance
            };
        }

        private class TypedBinding<TExodata, TContext, TSubject> : IExodataBinding, IExodataBindingDetails
        {
                private class CacheValue
                {
                    public TExodata Exodata { get; set; }
                    public int RequestHashCode { get; set; }
                }

                private readonly WeakKeyDictionary<object, ICollection<CacheValue>> _CacheDictionary;
                private readonly MultiMap<object, CacheValue> _Cache;

                public TypedBinding(Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate, Func<IExodataRequest<TExodata, TContext, TSubject>, TExodata> valueFactory, IExodataBindingSource source)
                {
                    Guard.NotNull(predicate, "predicate");
                    Guard.NotNull(valueFactory, "valueFactory");
                    Guard.NotNull(source, "source");

                    _CacheDictionary = new WeakKeyDictionary<object, ICollection<CacheValue>>();
                    _Cache = new MultiMap<object, CacheValue>(_CacheDictionary);

                    Predicate = predicate;
                    ValueFactory = valueFactory;
                    Source = source;
                }

                private bool Matches<TRequestExodata, TRequestContext, TRequestSubject>(IExodataRequest<TRequestExodata, TRequestContext, TRequestSubject> request)
                {
                    var predicate = Predicate as Func<IExodataRequest<TRequestExodata, TRequestContext, TRequestSubject>, bool>;
                    return predicate != null && predicate(request);
                }

                public Maybe<TRequestExodata> TryResolve<TRequestExodata, TRequestContext, TRequestSubject>(IExodataRequest<TRequestExodata, TRequestContext, TRequestSubject> request)
                {
                    if(Matches(request) != true)
                        return Maybe<TRequestExodata>.NoValue;

                    int requestHashCode = request.GetHashCode();

                    object scopeObject = ScopeFactory != null
                        ? (ScopeFactory as Func<IExodataRequest<TRequestExodata, TRequestContext, TRequestSubject>, object>)(request)
                        : null;

                    var exodataScopeObject = scopeObject as IExodataScopeObject;

                    if (scopeObject != null)
                    {
                        _CacheDictionary.PurgeGarbage(null);

                        var scopedCache = _Cache[scopeObject];
                        var cachedValue = scopedCache.FirstOrDefault(x => x.RequestHashCode == requestHashCode);

                        if (exodataScopeObject == null || exodataScopeObject.IsInScope(this, request))
                        {
                            if (cachedValue != null)
                                return Cast<TExodata, TRequestExodata>.With(cachedValue.Exodata);
                        }
                        else if (cachedValue != null)
                            _Cache.Remove(scopeObject, cachedValue);
                    }

                    var results = (ValueFactory as Func<IExodataRequest<TRequestExodata, TRequestContext, TRequestSubject>, TRequestExodata>)(request);

                    if (scopeObject != null)
                    {
                        _Cache.Add(scopeObject, new CacheValue {Exodata = Cast<TRequestExodata, TExodata>.With(results), RequestHashCode = requestHashCode});

                        if (exodataScopeObject != null)
                        {
                            exodataScopeObject.CacheFlushRequested += (s, a) => _CacheDictionary
                                                                            .TryGetValue(scopeObject)
                                                                            .OnValue(x => x.Clear())
                                                                            .OnValue(x => _CacheDictionary.Remove(scopeObject))
                                                                            .Run();
                        }
                    }

                    return results;
                }

                public Type ContextType { get; set; }
                public Type SubjectType { get; set; }
                public IExodataBindingSource Source { get; set; }

                public bool BoundToContextInstance { get; set; }
                public bool BoundToSubjectInstance { get; set; }

                public Func<IExodataRequest<TExodata, TContext, TSubject>, object> ScopeFactory { get; set; }
                public Func<IExodataRequest<TExodata, TContext, TSubject>, bool> Predicate { get; set; }
                public Func<IExodataRequest<TExodata, TContext, TSubject>, TExodata> ValueFactory { get; set; }
            }
    }
}
