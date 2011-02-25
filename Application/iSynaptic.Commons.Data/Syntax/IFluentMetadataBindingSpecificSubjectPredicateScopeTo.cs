using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, TSubject> : IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject>
    {
        IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For(Expression<Func<TSubject, object>> member);
        IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For(TSubject subject);
        IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member);

        IFluentMetadataBindingPredicateScopeTo<TMetadata, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject;
        IFluentMetadataBindingPredicateScopeTo<TMetadata, TDerivedSubject> For<TDerivedSubject>(Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject;
        IFluentMetadataBindingPredicateScopeTo<TMetadata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject;
        IFluentMetadataBindingPredicateScopeTo<TMetadata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject;
    }
}