// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
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
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public static class ExodataResolverExtensions
    {
        public static IFluentExodataResolutionRoot<TExodata> TryResolve<TExodata>(this IExodataResolver resolver, ISymbol<TExodata> symbol)
        {
            Guard.NotNull(resolver, "resolver");
            Guard.NotNull(symbol, "symbol");

            return new ExodataResolverFluentExodataResolutionRoot<TExodata, object>(resolver, symbol);
        }
    }

    internal class ExodataResolverFluentExodataResolutionRoot<TExodata, TContext> : FluentExodataResolutionRoot<TExodata, TContext>
    {
        private readonly ISymbol<TExodata> _Symbol;
        private readonly IExodataResolver _Resolver;

        public ExodataResolverFluentExodataResolutionRoot(IExodataResolver resolver, ISymbol<TExodata> symbol, Maybe<TContext> context = default(Maybe<TContext>))
            : base(context)
        {
            _Resolver = Guard.NotNull(resolver, "resolver");
            _Symbol = Guard.NotNull(symbol, "symbol");
        }

        protected override IFluentExodataResolutionRoot<TExodata> CreateNewResolutionRoot<TDesiredContext>(Maybe<TDesiredContext> context)
        {
            return new ExodataResolverFluentExodataResolutionRoot<TExodata, TDesiredContext>(_Resolver, _Symbol, context);
        }

        protected override TExodata Resolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return _Resolver.TryResolve(ExodataRequest.Create<TExodata, TContext, TSubject>(_Symbol, context, subject, member)).Value;
        }

        protected override Maybe<TExodata> TryResolve<TSubject>(Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return _Resolver.TryResolve(ExodataRequest.Create<TExodata, TContext, TSubject>(_Symbol, context, subject, member));
        }
    }

}
