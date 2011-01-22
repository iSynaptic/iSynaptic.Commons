using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface ISubjectPredicateScopeToBinding<TMetadata> : ISpecificSubjectPredicateScopeToBinding<TMetadata, object>
    {
        new IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>();
        new IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(Expression<Func<TSubject, object>> member);
        new IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(TSubject subject);
        new IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}