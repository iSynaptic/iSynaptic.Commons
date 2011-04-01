using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class FluentExodataBindingBuilder<TExodata, TContext, TSubject> : IFluentExodataBindingSubjectGivenWhenScopeTo<TExodata, TContext, TSubject>
    {
        public FluentExodataBindingBuilder(IExodataBindingSource source, IExodataDeclaration declaration, Action<IExodataBinding> onBuildComplete)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(declaration, "declaration");
            Guard.NotNull(onBuildComplete, "onBuildComplete");

            Source = source;
            Declaration = declaration;
            OnBuildComplete = onBuildComplete;

            Subject = Maybe<TSubject>.NoValue;
            Context = Maybe<TContext>.NoValue;
        }

        public IFluentExodataBindingGivenWhenScopeTo<TExodata, TContext, TSubject> For(TSubject subject)
        {
            return ForCore<TSubject>(subject, null);
        }

        public IFluentExodataBindingGivenWhenScopeTo<TExodata, TContext, TSubject> For(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");
            return ForCore(Maybe<TSubject>.NoValue, member);
        }

        public IFluentExodataBindingGivenWhenScopeTo<TExodata, TContext, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");
            return ForCore(subject, member);
        }

        public IFluentExodataBindingGivenWhenScopeTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject
        {
            return ForCore(Maybe<TDerivedSubject>.NoValue, null);
        }

        public IFluentExodataBindingGivenWhenScopeTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject
        {
            return ForCore<TDerivedSubject>(subject, null);
        }

        public IFluentExodataBindingGivenWhenScopeTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            Guard.NotNull(member, "member");
            return ForCore(Maybe<TDerivedSubject>.NoValue, member);
        }

        public IFluentExodataBindingGivenWhenScopeTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            Guard.NotNull(member, "member");
            return ForCore(subject, member);
        }

        private IFluentExodataBindingGivenWhenScopeTo<TExodata, TContext, TDerivedSubject> ForCore<TDerivedSubject>(Maybe<TDerivedSubject> subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            return new FluentExodataBindingBuilder<TExodata, TContext, TDerivedSubject>(Source, Declaration, OnBuildComplete)
            {
                Subject = subject,
                Member = member != null
                    ? member.ExtractMemberInfoForExodata<TDerivedSubject>()
                    : null
            };
        }

        public IFluentExodataBindingWhenScopeTo<TExodata, TDerivedContext, TSubject> Given<TDerivedContext>(Maybe<TDerivedContext> context) where TDerivedContext : TContext
        {
            return new FluentExodataBindingBuilder<TExodata, TDerivedContext, TSubject>(Source, Declaration, OnBuildComplete)
            {
                Subject = Subject,
                Member = Member,
                Context = context
            };
        }
        
        public IFluentExodataBindingScopeTo<TExodata, TContext, TSubject> When(Func<IExodataRequest<TContext, TSubject>, bool> userPredicate)
        {
            Guard.NotNull(userPredicate, "userPredicate");
            UserPredicate = userPredicate;
            return this;
        }

        public IFluentExodataBindingTo<TExodata, TContext, TSubject> InScope(object scopeObject)
        {
            Guard.NotNull(scopeObject, "scopeObject");
            ScopeFactory = r => scopeObject;
            return this;
        }

        public IFluentExodataBindingTo<TExodata, TContext, TSubject> InScope(Func<IExodataRequest<TContext, TSubject>, object> scopeFactory)
        {
            Guard.NotNull(scopeFactory, "scopeFactory");
            ScopeFactory = scopeFactory;
            return this;
        }

        public void To(TExodata value)
        {
            To(r => value);
        }

        public void To(Func<IExodataRequest<TContext, TSubject>, TExodata> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");

            OnBuildComplete(ExodataBinding.Create(Source, Matches, valueFactory, ScopeFactory, Context.HasValue, Subject.HasValue));
        }

        private bool Matches(IExodataRequest<TContext, TSubject> request)
        {
            Guard.NotNull(request, "request");

            if (Declaration != request.Declaration)
                return false;

            if (Member != request.Member)
                return false;

            if (Context.HasValue && !request.Context.HasValue)
                return false;

            if (Context.HasValue && !EqualityComparer<TContext>.Default.Equals(request.Context.Value, Context.Value))
                return false;

            if (Subject.HasValue && !request.Subject.HasValue)
                return false;

            if (Subject.HasValue && !EqualityComparer<TSubject>.Default.Equals(request.Subject.Value, Subject.Value))
                return false;

            if (UserPredicate != null)
                return UserPredicate(request);

            return true;
        }

        protected IExodataBindingSource Source { get; set; }

        protected Maybe<TContext> Context { get; set; }
        protected Maybe<TSubject> Subject { get; set; }
        protected MemberInfo Member { get; set; }

        protected IExodataDeclaration Declaration { get; set; }

        protected Func<IExodataRequest<TContext, TSubject>, bool> UserPredicate { get; set; }
        protected Func<IExodataRequest<TContext, TSubject>, object> ScopeFactory { get; set; }

        protected Action<IExodataBinding> OnBuildComplete { get; set; }
    }
}