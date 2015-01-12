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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using iSynaptic.Commons.Data.Syntax;

namespace iSynaptic.Commons.Data
{
    public class ExodataBindingModule : IExodataBindingSource, IFluentExodataBindingRoot<object, object>, IFluentInterface
    {
        private readonly HashSet<IExodataBinding> _Bindings = new HashSet<IExodataBinding>();

        public IEnumerable<IExodataBinding> GetBindingsFor<TExodata, TContext, TSubject>(IExodataRequest<TExodata, TContext, TSubject> request)
        {
            return _Bindings;
        }

        public IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, object, object> Bind<TExodata>(ISymbol<TExodata> symbol)
        {
            Guard.NotNull(symbol, "symbol");
            return Bind<TExodata>((ISymbol)symbol);
        }

        public IFluentExodataBindingNamedGivenSubjectWhenTo<TExodata, object, object> Bind<TExodata>(ISymbol symbol)
        {
            Guard.NotNull(symbol, "symbol");
            return new FluentExodataBindingBuilder<TExodata, object, object>(this, symbol.ToMaybe(), b => _Bindings.Add(b));
        }

        public void Bind<TExodata>(ISymbol<TExodata> symbol, TExodata value, string name = null)
        {
            Bind<TExodata>((ISymbol)symbol).Named(name).To(value);
        }

        public void Bind<TExodata>(ISymbol symbol, TExodata value, string name = null)
        {
            Bind<TExodata>(symbol).Named(name).To(value);
        }
    }
}
