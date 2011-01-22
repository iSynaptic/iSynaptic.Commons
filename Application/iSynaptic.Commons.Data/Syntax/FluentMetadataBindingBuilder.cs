using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class BaseFluentMetadataBindingBuilder<TMetadata, TSubject> : IPredicateScopeToBinding<TMetadata, TSubject>
    {
        public BaseFluentMetadataBindingBuilder(IMetadataBindingSource source, Func<MetadataRequest<TMetadata, TSubject>, bool> userPredicate, Action<MetadataBinding<TMetadata, TSubject>> onBuildComplete)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(userPredicate, "userPredicate");
            Guard.NotNull(onBuildComplete, "onBuildComplete");

            Source = source;
            UserPredicate = userPredicate;
            OnBuildComplete = onBuildComplete;

            Subject = Maybe<TSubject>.NoValue;
        }

        public void To(TMetadata value)
        {
            To(r => value);
        }

        public void To(Func<MetadataRequest<TMetadata, TSubject>, TMetadata> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");

            OnBuildComplete(new MetadataBinding<TMetadata, TSubject>(Matches, valueFactory, Source)
            {
                ScopeFactory = ScopeFactory,
                Subject = Subject,
                Member = Member
            });
        }

        public IScopeToBinding<TMetadata, TSubject> When(Func<MetadataRequest<TMetadata, TSubject>, bool> userPredicate)
        {
            Guard.NotNull(userPredicate, "userPredicate");
            UserPredicate = userPredicate;
            return this;
        }

        public IToBinding<TMetadata, TSubject> InScope(object scopeObject)
        {
            Guard.NotNull(scopeObject, "scopeObject");
            ScopeFactory = r => scopeObject;
            return this;
        }

        public IToBinding<TMetadata, TSubject> InScope(Func<MetadataRequest<TMetadata, TSubject>, object> scopeFactory)
        {
            Guard.NotNull(scopeFactory, "scopeFactory");
            ScopeFactory = scopeFactory;
            return this;
        }

        private bool Matches(MetadataRequest<TMetadata, TSubject> request)
        {
            Guard.NotNull(request, "request");

            if (Member != request.Member)
                return false;

            if (Subject.HasValue && !request.Subject.HasValue)
                return false;

            if (Subject.HasValue && !EqualityComparer<TSubject>.Default.Equals(request.Subject.Value, Subject.Value))
                return false;

            return UserPredicate(request);
        }

        protected IMetadataBindingSource Source { get; set; }
        protected Maybe<TSubject> Subject { get; set; }
        protected MemberInfo Member { get; set; }

        protected Func<MetadataRequest<TMetadata, TSubject>, bool> UserPredicate { get; set; }
        protected Func<MetadataRequest<TMetadata, TSubject>, object> ScopeFactory { get; set; }

        protected Action<MetadataBinding<TMetadata, TSubject>> OnBuildComplete { get; set; }
    }

    internal class FluentMetadataBindingBuilder<TMetadata, TSubject> : BaseFluentMetadataBindingBuilder<TMetadata, TSubject>, ISpecificSubjectPredicateScopeToBinding<TMetadata, TSubject>
    {
        public FluentMetadataBindingBuilder(IMetadataBindingSource source, Func<MetadataRequest<TMetadata, TSubject>, bool> userPredicate, Action<MetadataBinding<TMetadata, TSubject>> onBuildComplete)
            : base(source, userPredicate, onBuildComplete)
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
    }
}
