using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, TSubject> : IFluentExodataBindingPredicateScopeTo<TExodata, TSubject>
    {
        IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For(Expression<Func<TSubject, object>> member);
        IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For(TSubject subject);
        IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member);

        IFluentExodataBindingPredicateScopeTo<TExodata, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject;
        IFluentExodataBindingPredicateScopeTo<TExodata, TDerivedSubject> For<TDerivedSubject>(Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject;
        IFluentExodataBindingPredicateScopeTo<TExodata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject;
        IFluentExodataBindingPredicateScopeTo<TExodata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject;
    }
}