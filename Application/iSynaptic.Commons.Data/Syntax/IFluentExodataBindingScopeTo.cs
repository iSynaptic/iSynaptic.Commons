using System;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingScopeTo<TExodata, TSubject> : IFluentExodataBindingTo<TExodata, TSubject>
    {
        IFluentExodataBindingTo<TExodata, TSubject> InScope(object scopeObject);
        IFluentExodataBindingTo<TExodata, TSubject> InScope(Func<IExodataRequest<TSubject>, object> scopeFactory);
    }
}