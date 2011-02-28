using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> : IFluentExodataBindingScopeTo<TExodata, TSubject>
    {
        IFluentExodataBindingScopeTo<TExodata, TSubject> When(Func<IExodataRequest<TSubject>, bool> predicate);
    }
}