using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataBindingSubjectPredicateScopeTo<TExodata> : IFluentExodataBindingSpecificSubjectPredicateScopeTo<TExodata, object>
    {
        new IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For<TSubject>();
        new IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For<TSubject>(Expression<Func<TSubject, object>> member);
        new IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For<TSubject>(TSubject subject);
        new IFluentExodataBindingPredicateScopeTo<TExodata, TSubject> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}