using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class FluentExodataBindingBuilder<TExodata, TContext, TSubject> : IFluentExodataBindingGivenSubjectWhenScopeTo<TExodata, TContext, TSubject>
    {
        public FluentExodataBindingBuilder(IExodataBindingSource source, IExodataDeclaration<TExodata> declaration, Action<IExodataBinding> onBuildComplete)
        {
            Guard.NotNull(source, "source");
            Guard.NotNull(declaration, "declaration");
            Guard.NotNull(onBuildComplete, "onBuildComplete");

            Source = source;
            Declaration = declaration;
            OnBuildComplete = onBuildComplete;

            Context = Maybe<TContext>.NoValue;
            Subject = Maybe<TSubject>.NoValue;
        }


        public IFluentExodataBindingSubjectWhenScopeTo<TExodata, TDerivedContext, TSubject> Given<TDerivedContext>() where TDerivedContext : TContext
        {
            return new FluentExodataBindingBuilder<TExodata, TDerivedContext, TSubject>(Source, Declaration, OnBuildComplete)
            {
                Context = Maybe<TDerivedContext>.NoValue
            };
        }

        public IFluentExodataBindingSubjectWhenScopeTo<TExodata, TDerivedContext, TSubject> Given<TDerivedContext>(TDerivedContext context) where TDerivedContext : TContext
        {
            return new FluentExodataBindingBuilder<TExodata, TDerivedContext, TSubject>(Source, Declaration, OnBuildComplete)
            {
                Context = context
            };
        }

        public IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> For(TSubject subject)
        {
            return ForCore<TSubject>(subject, null);
        }

        public IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> For(Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");
            return ForCore(Maybe<TSubject>.NoValue, member);
        }

        public IFluentExodataBindingWhenScopeTo<TExodata, TContext, TSubject> For(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            Guard.NotNull(member, "member");
            return ForCore(subject, member);
        }

        public IFluentExodataBindingWhenScopeTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject
        {
            return ForCore(Maybe<TDerivedSubject>.NoValue, null);
        }

        public IFluentExodataBindingWhenScopeTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject
        {
            return ForCore<TDerivedSubject>(subject, null);
        }

        public IFluentExodataBindingWhenScopeTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            Guard.NotNull(member, "member");
            return ForCore(Maybe<TDerivedSubject>.NoValue, member);
        }

        public IFluentExodataBindingWhenScopeTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            Guard.NotNull(member, "member");
            return ForCore(subject, member);
        }

        private IFluentExodataBindingWhenScopeTo<TExodata, TContext, TDerivedSubject> ForCore<TDerivedSubject>(Maybe<TDerivedSubject> subject, Expression<Func<TDerivedSubject, object>> member) where TDerivedSubject : TSubject
        {
            return new FluentExodataBindingBuilder<TExodata, TContext, TDerivedSubject>(Source, Declaration, OnBuildComplete)
            {
                Context = Context,
                Subject = subject,
                Member = member != null
                    ? member.ExtractMemberInfoForExodata<TDerivedSubject>()
                    : null
            };
        }

        public IFluentExodataBindingScopeTo<TExodata, TContext, TSubject> When(Func<IExodataRequest<TExodata, TContext, TSubject>, bool> userPredicate)
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

        public IFluentExodataBindingTo<TExodata, TContext, TSubject> InScope(Func<IExodataRequest<TExodata, TContext, TSubject>, object> scopeFactory)
        {
            Guard.NotNull(scopeFactory, "scopeFactory");
            ScopeFactory = scopeFactory;
            return this;
        }

        public void To(TExodata value)
        {
            To(r => value);
        }

        public void To(Func<IExodataRequest<TExodata, TContext, TSubject>, TExodata> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");

            OnBuildComplete(ExodataBinding.Create<TExodata, TContext, TSubject>(Source, Matches, valueFactory, ScopeFactory, Context.HasValue, Subject.HasValue));
        }

        private bool Matches(IExodataRequest<TExodata, TContext, TSubject> request)
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

        protected IExodataDeclaration<TExodata> Declaration { get; set; }

        protected Func<IExodataRequest<TExodata, TContext, TSubject>, bool> UserPredicate { get; set; }
        protected Func<IExodataRequest<TExodata, TContext, TSubject>, object> ScopeFactory { get; set; }

        protected Action<IExodataBinding> OnBuildComplete { get; set; }
    }
}