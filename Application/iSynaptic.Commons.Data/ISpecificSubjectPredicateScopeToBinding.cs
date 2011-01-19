using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data
{
    public interface ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject> : IPredicateScopeToBinding<TMetadata>
    {
        IPredicateScopeToBinding<TMetadata> For(Expression<Func<TSubject, object>> member);
        IPredicateScopeToBinding<TMetadata> For(TSubject subject);
        IPredicateScopeToBinding<TMetadata> For(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}