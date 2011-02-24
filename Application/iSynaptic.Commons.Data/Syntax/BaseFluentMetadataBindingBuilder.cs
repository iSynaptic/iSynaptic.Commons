using System;
using System.Collections.Generic;
using System.Reflection;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class BaseFluentMetadataBindingBuilder<TMetadata, TSubject> : IPredicateScopeToBinding<TMetadata, TSubject>
    {
        public BaseFluentMetadataBindingBuilder(IMetadataBindingSource source, IMetadataDeclaration declaration, Action<IMetadataBinding> onBuildComplete)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(declaration, "declaration");
            Guard.NotNull(onBuildComplete, "onBuildComplete");

            Source = source;
            Declaration = declaration;
            OnBuildComplete = onBuildComplete;

            Subject = Maybe<TSubject>.NoValue;
        }

        public void To(TMetadata value)
        {
            To(r => value);
        }

        public void To(Func<IMetadataRequest<TSubject>, TMetadata> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");

            OnBuildComplete(MetadataBinding.Create(Source, Matches, valueFactory, ScopeFactory, Subject.HasValue));
        }

        public IScopeToBinding<TMetadata, TSubject> When(Func<IMetadataRequest<TSubject>, bool> userPredicate)
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

        public IToBinding<TMetadata, TSubject> InScope(Func<IMetadataRequest<TSubject>, object> scopeFactory)
        {
            Guard.NotNull(scopeFactory, "scopeFactory");
            ScopeFactory = scopeFactory;
            return this;
        }

        private bool Matches(IMetadataRequest<TSubject> request)
        {
            Guard.NotNull(request, "request");

            if (Declaration != request.Declaration)
                return false;

            if (Member != request.Member)
                return false;

            if (Subject.HasValue && !request.Subject.HasValue)
                return false;

            if (Subject.HasValue && !EqualityComparer<TSubject>.Default.Equals(request.Subject.Value, Subject.Value))
                return false;

            if(UserPredicate != null)
                return UserPredicate(request);

            return true;
        }

        protected IMetadataBindingSource Source { get; set; }
        protected Maybe<TSubject> Subject { get; set; }
        protected MemberInfo Member { get; set; }

        protected IMetadataDeclaration Declaration { get; set; }

        protected Func<IMetadataRequest<TSubject>, bool> UserPredicate { get; set; }
        protected Func<IMetadataRequest<TSubject>, object> ScopeFactory { get; set; }

        protected Action<IMetadataBinding> OnBuildComplete { get; set; }
    }
}