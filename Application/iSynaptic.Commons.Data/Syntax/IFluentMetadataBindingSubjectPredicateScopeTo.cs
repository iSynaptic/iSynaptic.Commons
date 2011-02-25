using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentMetadataBindingSubjectPredicateScopeTo<TMetadata> : IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, object>
    {
        new IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For<TSubject>();
        new IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For<TSubject>(Expression<Func<TSubject, object>> member);
        new IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For<TSubject>(TSubject subject);
        new IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}