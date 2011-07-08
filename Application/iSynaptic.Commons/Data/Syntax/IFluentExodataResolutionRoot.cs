using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataResolutionRoot<TExodata>
    {
        IFluentExodataResolutionRoot<TExodata> Given<TContext>();
        IFluentExodataResolutionRoot<TExodata> Given<TContext>(TContext context);

        Maybe<TExodata> TryGet();
        Maybe<TExodata> TryFor<TSubject>();
        Maybe<TExodata> TryFor<TSubject>(TSubject subject);
        Maybe<TExodata> TryFor<TSubject>(Expression<Func<TSubject, object>> member);
        Maybe<TExodata> TryFor<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);

        TExodata Get();
        TExodata For<TSubject>();
        TExodata For<TSubject>(TSubject subject);
        TExodata For<TSubject>(Expression<Func<TSubject, object>> member);
        TExodata For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}
