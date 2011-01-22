using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface ISubjectPredicateScopeToBinding<TMetadata> : ISpecificSubjectPredicateScopeToBinding<TMetadata, object>
    {
        IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>();
        IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(Expression<Func<TSubject, object>> member);
        IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(TSubject subject);
        IPredicateScopeToBinding<TMetadata, TSubject> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}