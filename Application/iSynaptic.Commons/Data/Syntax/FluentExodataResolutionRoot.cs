﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data.Syntax
{
    public abstract class FluentExodataResolutionRoot<TExodata, TContext> : IFluentExodataResolutionRoot<TExodata>
    {
        private readonly Maybe<TContext> _Context;

        protected FluentExodataResolutionRoot(Maybe<TContext> context = default(Maybe<TContext>))
        {
            _Context = context;
        }

        public IFluentExodataResolutionRoot<TExodata> Given<TDesiredContext>()
        {
            return CreateNewResolutionRoot(Maybe<TDesiredContext>.NoValue);
        }

        public IFluentExodataResolutionRoot<TExodata> Given<TDesiredContext>(TDesiredContext context)
        {
            return CreateNewResolutionRoot(context.ToMaybe());
        }

        public Maybe<TExodata> TryGet()
        {
            return TryResolve(Maybe<object>.NoValue, null);
        }

        public Maybe<TExodata> TryFor<TSubject>()
        {
            return TryResolve(Maybe<TSubject>.NoValue, null);
        }

        public Maybe<TExodata> TryFor<TSubject>(TSubject subject)
        {
            return TryResolve(subject.ToMaybe(), null);
        }

        public Maybe<TExodata> TryFor<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return TryResolve(Maybe<TSubject>.NoValue, member);
        }

        public Maybe<TExodata> TryFor<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return TryResolve(subject.ToMaybe(), member);
        }

        private Maybe<TExodata> TryResolve<TSubject>(Maybe<TSubject> subject, Expression memberExpression)
        {
            MemberInfo member = memberExpression != null
                ? memberExpression.ExtractMemberInfoForExodata<TSubject>()
                : null;

            return TryResolve(_Context, subject, member);
        }

        public TExodata Get()
        {
            return Resolve(Maybe<object>.NoValue, null);
        }

        public TExodata For<TSubject>()
        {
            return Resolve(Maybe<TSubject>.NoValue, null);
        }

        public TExodata For<TSubject>(TSubject subject)
        {
            return Resolve(subject.ToMaybe(), null);
        }

        public TExodata For<TSubject>(Expression<Func<TSubject, object>> member)
        {
            return Resolve(Maybe<TSubject>.NoValue, member);
        }

        public TExodata For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member)
        {
            return Resolve(subject.ToMaybe(), member);
        }

        private TExodata Resolve<TSubject>(Maybe<TSubject> subject, Expression memberExpression)
        {
            MemberInfo member = memberExpression != null
                ? memberExpression.ExtractMemberInfoForExodata<TSubject>()
                : null;

            return Resolve(_Context, subject, member);
        }

        protected abstract IFluentExodataResolutionRoot<TExodata> CreateNewResolutionRoot<TDesiredContext>(Maybe<TDesiredContext> context);

        protected abstract TExodata Resolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member);
        protected abstract Maybe<TExodata> TryResolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member);
    }
}