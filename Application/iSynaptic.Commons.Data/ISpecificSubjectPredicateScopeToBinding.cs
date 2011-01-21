using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data
{
    public interface ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject> : IPredicateScopeToBinding<TMetadata, TSubject>
    {
        IPredicateScopeToBinding<TMetadata, TSubject> For(Expression<Func<TSubject, object>> member);
        IPredicateScopeToBinding<TMetadata, TSubject> For(TSubject subject);
        IPredicateScopeToBinding<TMetadata, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}