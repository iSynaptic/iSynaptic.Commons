// The MIT License
// 
// Copyright (c) 2012 Jordan E. Terrell
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace iSynaptic.Commons.Data.Syntax
{
    internal class FluentExodataBindingBuilder<TExodata, TContext, TSubject> : IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, TContext, TSubject>
    {
        public FluentExodataBindingBuilder(IExodataBindingSource source, Maybe<ISymbol> symbol, Action<IExodataBinding> onBuildComplete)
        {
            Source = Guard.NotNull(source, "source");
            Symbol = symbol;
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
            return ForCore(subject.ToMaybe(), null);
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
            return ForCore(subject.ToMaybe(), null);
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
            Predicate = Guard.NotNull(predicate, "predicate");
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
            OnBuildComplete(ExodataBinding.Create(Name, Source, Symbol, Context, Subject, Members, Predicate ?? (r => true), valueFactory));
        }

        protected string Name { get; set; }
        protected IExodataBindingSource Source { get; set; }

        protected Maybe<ISymbol> Symbol { get; set; }
        protected Maybe<TContext> Context { get; set; }
        protected Maybe<TSubject> Subject { get; set; }
        protected MemberInfo[] Members { get; set; }

        protected Func<IExodataRequest<TExodata, TContext, TSubject>, bool> Predicate { get; set; }

        protected Action<IExodataBinding> OnBuildComplete { get; set; }
    }
}