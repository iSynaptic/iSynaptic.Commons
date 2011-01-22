using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class FluentMetadataBindingBuilder<TMetadata, TSubject> : BaseFluentMetadataBindingBuilder<TMetadata, TSubject>, ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject>
    {
        public FluentMetadataBindingBuilder(IMetadataBindingSource source, MetadataDeclaration<TMetadata> declaration, Action<object> onBuildComplete)
            : base(source, declaration, onBuildComplete)
        {
        }

        public IPredicateScopeToBinding<TMetadata, TSubject> For(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");
            Member = member.ExtractMemberInfoForMetadata();

            return this;
        }

        public IPredicateScopeToBinding<TMetadata, TSubject> For(TSubject subject)
        {
            Subject = subject;

            return this;
        }

        public IPredicateScopeToBinding<TMetadata, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");

            Subject = subject;
            Member = member.ExtractMemberInfoForMetadata();

            return this;
        }

        public IPredicateScopeToBinding<TMetadata, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>();
        }

        public IPredicateScopeToBinding<TMetadata, TDerivedSubject> For<TDerivedSubject>(Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(member);
        }

        public IPredicateScopeToBinding<TMetadata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(subject);
        }

        public IPredicateScopeToBinding<TMetadata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(subject, member);
        }

        private ISpecificSubjectPredicateScopeToBinding<TMetadata, TDerivedSubject> UseDerivedSubject<TDerivedSubject>()
        {
            return new FluentMetadataBindingBuilder<TMetadata, TDerivedSubject>(Source, Declaration, OnBuildComplete);
        }

    }
}
