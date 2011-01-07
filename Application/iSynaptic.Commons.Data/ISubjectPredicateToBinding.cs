using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data
{
    public interface ISubjectPredicateToBinding<TMetadata> : IPredicateScopeToBinding<TMetadata>
    {
        IPredicateScopeToBinding<TMetadata> For<TSubject>();
        IPredicateScopeToBinding<TMetadata> For<TSubject>(Expression<Func<TSubject, object>> member);
        IPredicateScopeToBinding<TMetadata> For<TSubject>(TSubject subject);
        IPredicateScopeToBinding<TMetadata> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}