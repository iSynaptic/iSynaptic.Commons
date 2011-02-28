using System;
using System.Collections.Generic;
using System.Reflection;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class BaseFluentExodataBindingBuilder<TExodata, TSubject> : IFluentExodataBindingPredicateScopeTo<TExodata, TSubject>
    {
        public BaseFluentExodataBindingBuilder(IExodataBindingSource source, IExodataDeclaration declaration, Action<IExodataBinding> onBuildComplete)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(declaration, "declaration");
            Guard.NotNull(onBuildComplete, "onBuildComplete");

            Source = source;
            Declaration = declaration;
            OnBuildComplete = onBuildComplete;

            Subject = Maybe<TSubject>.NoValue;
        }

        public void To(TExodata value)
        {
            To(r => value);
        }

        public void To(Func<IExodataRequest<TSubject>, TExodata> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");

            OnBuildComplete(ExodataBinding.Create(Source, Matches, valueFactory, ScopeFactory, Subject.HasValue));
        }

        public IFluentExodataBindingScopeTo<TExodata, TSubject> When(Func<IExodataRequest<TSubject>, bool> userPredicate)
        {
            Guard.NotNull(userPredicate, "userPredicate");
            UserPredicate = userPredicate;
            return this;
        }

        public IFluentExodataBindingTo<TExodata, TSubject> InScope(object scopeObject)
        {
            Guard.NotNull(scopeObject, "scopeObject");
            ScopeFactory = r => scopeObject;
            return this;
        }

        public IFluentExodataBindingTo<TExodata, TSubject> InScope(Func<IExodataRequest<TSubject>, object> scopeFactory)
        {
            Guard.NotNull(scopeFactory, "scopeFactory");
            ScopeFactory = scopeFactory;
            return this;
        }

        private bool Matches(IExodataRequest<TSubject> request)
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

        protected IExodataBindingSource Source { get; set; }
        protected Maybe<TSubject> Subject { get; set; }
        protected MemberInfo Member { get; set; }

        protected IExodataDeclaration Declaration { get; set; }

        protected Func<IExodataRequest<TSubject>, bool> UserPredicate { get; set; }
        protected Func<IExodataRequest<TSubject>, object> ScopeFactory { get; set; }

        protected Action<IExodataBinding> OnBuildComplete { get; set; }
    }
}