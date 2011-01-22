using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject> : IPredicateScopeToBinding<TMetadata, TSubject>
    {
        IPredicateScopeToBinding<TMetadata, TSubject> For(Expression<Func<TSubject, object>> member);
        IPredicateScopeToBinding<TMetadata, TSubject> For(TSubject subject);
        IPredicateScopeToBinding<TMetadata, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member);

        IPredicateScopeToBinding<TMetadata, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject;
        IPredicateScopeToBinding<TMetadata, TDerivedSubject> For<TDerivedSubject>(Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject;
        IPredicateScopeToBinding<TMetadata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject;
        IPredicateScopeToBinding<TMetadata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject;
    }
}