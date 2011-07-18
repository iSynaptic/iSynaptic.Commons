using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class FluentExodataBindingBuilder<TExodata, TContext, TSubject> : IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, TContext, TSubject>
    {
        public FluentExodataBindingBuilder(IExodataBindingSource source, ISymbol symbol, Action<IExodataBinding> onBuildComplete)
        {
            Source = Guard.NotNull(source, "source");
            Symbol = Guard.NotNull(symbol, "symbol");
            OnBuildComplete = Guard.NotNull(onBuildComplete, "onBuildComplete");

            Context = Maybe<TContext>.NoValue;
            Subject = Maybe<TSubject>.NoValue;
            Members = new MemberInfo[0];
        }

        public IFluentExodataBindingGivenSubjectWhenTo<TExodata, TContext, TSubject> Named(string name)
        {
            Name = name;
            return this;
        }

        public IFluentExodataBindingSubjectWhenTo<TExodata, TDerivedContext, TSubject> Given<TDerivedContext>() where TDerivedContext : TContext
        {
            return new FluentExodataBindingBuilder<TExodata, TDerivedContext, TSubject>(Source, Symbol, OnBuildComplete)
            {
                Context = Maybe<TDerivedContext>.NoValue
            };
        }

        public IFluentExodataBindingSubjectWhenTo<TExodata, TDerivedContext, TSubject> Given<TDerivedContext>(TDerivedContext context) where TDerivedContext : TContext
        {
            return new FluentExodataBindingBuilder<TExodata, TDerivedContext, TSubject>(Source, Symbol, OnBuildComplete)
            {
                Context = context.ToMaybe()
            };
        }

        public IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For(TSubject subject)
        {
            return ForCore<TSubject>(subject.ToMaybe(), null);
        }

        public IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For(params Expression<Func<TSubject, object>>[] members)
        {
            Guard.NotNullOrEmpty(members, "members");
            return ForCore(Maybe<TSubject>.NoValue, members);
        }

        public IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For(TSubject subject, params Expression<Func<TSubject, object>>[] members)
        {
            Guard.NotNullOrEmpty(members, "members");
            return ForCore(subject.ToMaybe(), members);
        }

        public IFluentExodataBindingWhenTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>() where TDerivedSubject : TSubject
        {
            return ForCore(Maybe<TDerivedSubject>.NoValue, null);
        }

        public IFluentExodataBindingWhenTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject) where TDerivedSubject : TSubject
        {
            return ForCore<TDerivedSubject>(subject.ToMaybe(), null);
        }

        public IFluentExodataBindingWhenTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(params Expression<Func<TDerivedSubject, object>>[] members) where TDerivedSubject : TSubject
        {
            Guard.NotNullOrEmpty(members, "members");
            return ForCore(Maybe<TDerivedSubject>.NoValue, members);
        }

        public IFluentExodataBindingWhenTo<TExodata, TContext, TDerivedSubject> For<TDerivedSubject>(TDerivedSubject subject, params Expression<Func<TDerivedSubject, object>>[] members) where TDerivedSubject : TSubject
        {
            Guard.NotNullOrEmpty(members, "members");
            return ForCore(subject.ToMaybe(), members);
        }

        private IFluentExodataBindingWhenTo<TExodata, TContext, TDerivedSubject> ForCore<TDerivedSubject>(Maybe<TDerivedSubject> subject, IEnumerable<Expression<Func<TDerivedSubject, object>>> members) where TDerivedSubject : TSubject
        {
            return new FluentExodataBindingBuilder<TExodata, TContext, TDerivedSubject>(Source, Symbol, OnBuildComplete)
            {
                Context = Context,
                Subject = subject,
                Members = members != null
                    ? members.Select(x => x.ExtractMemberInfoForExodata<TDerivedSubject>()).ToArray()
                    : new MemberInfo[0]
            };
        }

        public IFluentExodataBindingTo<TExodata, TContext, TSubject> When(Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate)
        {
            WhenPredicate = Guard.NotNull(predicate, "predicate");
            return this;
        }

        public void To(TExodata value)
        {
            To(r => value);
        }

        public void To(Func<IExodataRequest<TExodata, TContext, TSubject>, TExodata> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");
            To(r => valueFactory(r).ToMaybe());
        }

        public void To(Func<IExodataRequest<TExodata, TContext, TSubject>, Maybe<TExodata>> valueFactory)
        {
            Guard.NotNull(valueFactory, "valueFactory");
            OnBuildComplete(ExodataBinding.Create(Source, Matches, valueFactory, Name, Context.HasValue, Subject.HasValue));
        }

        private bool Matches(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            Guard.NotNull(request, "request");

            if (Symbol != request.Symbol)
                return false;

            if (request.Member == null && Members.Length > 0)
                return false;

            if (request.Member != null && (Members == null || Members.Contains(request.Member) != true))
                return false;

            if (Context.HasValue && !request.Context.HasValue)
                return false;

            if (Context.HasValue && !EqualityComparer<TContext>.Default.Equals(request.Context.Value, Context.Value))
                return false;

            if (Subject.HasValue && !request.Subject.HasValue)
                return false;

            if (Subject.HasValue && !EqualityComparer<TSubject>.Default.Equals(request.Subject.Value, Subject.Value))
                return false;

            if (WhenPredicate != null)
                return WhenPredicate(request);

            return true;
        }

        protected string Name { get; set; }
        protected IExodataBindingSource Source { get; set; }

        protected Maybe<TContext> Context { get; set; }
        protected Maybe<TSubject> Subject { get; set; }
        protected MemberInfo[] Members { get; set; }

        protected ISymbol Symbol { get; set; }

        protected Func<IExodataRequest<TExodata, TContext, TSubject>, bool> WhenPredicate { get; set; }
        protected Action<IExodataBinding> OnBuildComplete { get; set; }
    }
}