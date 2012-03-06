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
using System.Linq.Expressions;

namespace iSynaptic.Commons.Data.Syntax
{
    public interface IFluentExodataResolutionRoot<TExodata>
    {
        IFluentExodataResolutionRoot<TExodata> Given<TContext>();
        IFluentExodataResolutionRoot<TExodata> Given<TContext>(TContext context);

        Maybe<TExodata> TryGet();
        Maybe<TExodata> TryFor<TSubject>();
        Maybe<TExodata> TryFor<TSubject>(TSubject subject);
        Maybe<TExodata> TryFor<TSubject>(Expression<Func<TSubject, object>> member);
        Maybe<TExodata> TryFor<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);

        TExodata Get();
        TExodata For<TSubject>();
        TExodata For<TSubject>(TSubject subject);
        TExodata For<TSubject>(Expression<Func<TSubject, object>> member);
        TExodata For<TSubject>(TSubject subject, Expression<Func<TSubject, object>> member);
    }
}
