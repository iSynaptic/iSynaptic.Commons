using System;
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class FluentMetadataBindingBuilder<TMetadata, TSubject> : BaseFluentMetadataBindingBuilder<TMetadata, TSubject>, IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, TSubject>
    {
        public FluentMetadataBindingBuilder(IMetadataBindingSource source, IMetadataDeclaration declaration, Action<IMetadataBinding> onBuildComplete)
            : base(source, declaration, onBuildComplete)
        {
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");
            Member = member.ExtractMemberInfoForMetadata();

            return this;
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For(TSubject subject)
        {
            Subject = subject;

            return this;
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");

            Subject = subject;
            Member = member.ExtractMemberInfoForMetadata();

            return this;
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>();
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TDerivedSubject> For<TDerivedSubject>(Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(member);
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(subject);
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            return UseDerivedSubject<TDerivedSubject>()
                .For(subject, member);
        }

        private IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, TDerivedSubject> UseDerivedSubject<TDerivedSubject>()
        {
            return new FluentMetadataBindingBuilder<TMetadata, TDerivedSubject>(Source, Declaration, OnBuildComplete);
        }

    }

    internal class FluentMetadataBindingBuilder<TMetadata> : BaseFluentMetadataBindingBuilder<TMetadata, object>, IFluentMetadataBindingSubjectPredicateScopeTo<TMetadata>
    {
        public FluentMetadataBindingBuilder(IMetadataBindingSource source, IMetadataDeclaration declaration, Action<IMetadataBinding> onBuildComplete)
            : base(source, declaration, onBuildComplete)
        {
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For<TSubject>()
        {
            return UseSpecificSubject<TSubject>();
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For<TSubject>(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");

            return UseSpecificSubject<TSubject>()
                .For(member);
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For<TSubject>(TSubject subject)
        {
            return UseSpecificSubject<TSubject>()
                .For(subject);
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, TSubject> For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");

            return UseSpecificSubject<TSubject>()
                .For(subject, member);
        }

        private IFluentMetadataBindingSpecificSubjectPredicateScopeTo<TMetadata, TSubject> UseSpecificSubject<TSubject>()
        {
            return new FluentMetadataBindingBuilder<TMetadata, TSubject>(Source, Declaration, OnBuildComplete);
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, object> For(Expression<Func<object, object>> member)
        {
            Guard.NotNull(member, "member");

            Member = member.ExtractMemberInfoForMetadata();
            return this;
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, object> For(object subject)
        {
            Subject = subject;
            return this;
        }

        public IFluentMetadataBindingPredicateScopeTo<TMetadata, object> For(object subject, Expression<Func<object, object>> member)
        {
            Guard.NotNull(member, "member");

            Subject = subject;
            Member = member.ExtractMemberInfoForMetadata();

            return this;
        }
    }
}


