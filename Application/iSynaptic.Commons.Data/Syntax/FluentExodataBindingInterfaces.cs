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
    public interface IFluentExodataBindingRoot<TContextBase, TSubjectBase>
    {
        IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> Bind<TExodata>(ISymbol<TExodata> symbol);
        IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> Bind<TExodata>(ISymbol symbol);

        void Bind<TExodata>(ISymbol<TExodata> symbol, TExodata value, string name = null);
        void Bind<TExodata>(ISymbol symbol, TExodata value, string name = null);
    }

    public interface IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> : IFluentExodataBindingGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase>
    {
        IFluentExodataBindingGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> Named(string name);
    }

    public interface IFluentExodataBindingGivenSubjectWhenTo<TExodata, TContextBase, TSubjectBase> : IFluentExodataBindingSubjectWhenTo<TExodata, TContextBase, TSubjectBase>
    {
        IFluentExodataBindingSubjectWhenTo<TExodata, TContext, TSubjectBase> Given<TContext>() where TContext : TContextBase;
        IFluentExodataBindingSubjectWhenTo<TExodata, TContext, TSubjectBase> Given<TContext>(TContext context) where TContext : TContextBase;
    }

    public interface IFluentExodataBindingSubjectWhenTo<TExodata, TContext, TSubjectBase> : IFluentExodataBindingWhenTo<TExodata, TContext, TSubjectBase>
    {
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubjectBase> For(TSubjectBase subject);
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubjectBase> For(params Expression<Func<TSubjectBase, object>>[] members);
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubjectBase> For(TSubjectBase subject, params Expression<Func<TSubjectBase, object>>[] members);

        IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For<TSubject>() where TSubject : TSubjectBase;
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For<TSubject>(TSubject subject) where TSubject : TSubjectBase;
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For<TSubject>(params Expression<Func<TSubject, object>>[] members) where TSubject : TSubjectBase;
        IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> For<TSubject>(TSubject subject, params Expression<Func<TSubject, object>>[] members) where TSubject : TSubjectBase;
    }

    public interface IFluentExodataBindingWhenTo<TExodata, TContext, TSubject> : IFluentExodataBindingTo<TExodata, TContext, TSubject>
    {
        IFluentExodataBindingTo<TExodata, TContext, TSubject> When(Func<IExodataRequest<TExodata, TContext, TSubject>, bool> predicate);
    }

    public interface IFluentExodataBindingTo<TExodata, TContext, TSubject> : IFluentInterface
    {
        void To(TExodata value);
        void To(Func<IExodataRequest<TExodata, TContext, TSubject>, TExodata> valueFactory);
        void To(Func<IExodataRequest<TExodata, TContext, TSubject>, Maybe<TExodata>> valueFactory);
    }
}
