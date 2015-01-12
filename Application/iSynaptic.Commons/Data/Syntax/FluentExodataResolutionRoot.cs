// The MIT License
// 
// Copyright (c) 2012-2015 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
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
