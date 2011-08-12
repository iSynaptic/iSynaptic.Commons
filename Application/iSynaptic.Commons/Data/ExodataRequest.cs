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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;

namespace iSynaptic.Commons.Data
{
    public static class ExodataRequest
    {
        public static ExodataRequest<TExodata, TContext, TSubject> Create<TExodata, TContext, TSubject>(ISymbol symbol, Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            return new ExodataRequest<TExodata, TContext, TSubject>(symbol, context, subject, member);
        }
    }

    public class ExodataRequest<TExodata, TContext, TSubject> : IExodataRequest<TExodata, TContext, TSubject>
    {
        private class RequestAdapter<TSourceExodata, TSourceContext, TSourceSubject>
            : IExodataRequest<TExodata, TContext, TSubject>
        {
            private readonly IExodataRequest<TSourceExodata, TSourceContext, TSourceSubject> _Request;

            public RequestAdapter(IExodataRequest<TSourceExodata, TSourceContext, TSourceSubject> request)
            {
                _Request = request;
            }

            public ISymbol Symbol
            {
                get { return _Request.Symbol; }
            }

            public Maybe<TContext> Context
            {
                get { return _Request.Context.Cast<TContext>(); }
            }

            public Maybe<TSubject> Subject
            {
                get { return _Request.Subject.Cast<TSubject>(); }
            }

            public MemberInfo Member
            {
                get { return _Request.Member; }
            }
        }

        public ExodataRequest(ISymbol symbol, Maybe<TContext> context, Maybe<TSubject> subject, MemberInfo member)
        {
            Symbol = Guard.NotNull(symbol, "symbol");
            Context = context;
            Subject = subject;
            Member = member;
        }

        public static Maybe<IExodataRequest<TExodata, TContext, TSubject>> TryToAdapt<TSourceExodata, TSourceContext, TSourceSubject>(IExodataRequest<TSourceExodata, TSourceContext, TSourceSubject> request)
        {
            Guard.NotNull(request, "request");

            var typedRequest = request as IExodataRequest<TExodata, TContext, TSubject>;
            if (typedRequest != null)
                return typedRequest.ToMaybe();

            if (typeof(TSourceExodata).IsAssignableFrom(typeof(TExodata)) &&
                typeof(TContext).IsAssignableFrom(typeof(TSourceContext)) &&
                typeof(TSubject).IsAssignableFrom(typeof(TSourceSubject)))
            {
                return new RequestAdapter<TSourceExodata, TSourceContext, TSourceSubject>(request)
                    .ToMaybe<IExodataRequest<TExodata, TContext, TSubject>>();
            }

            return Maybe<IExodataRequest<TExodata, TContext, TSubject>>.NoValue;
        }

        public ISymbol Symbol { get; private set; }

        public Maybe<TContext> Context { get; private set; }
        public Maybe<TSubject> Subject { get; private set; }
        public MemberInfo Member { get; private set; }
    }
}
