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
using System.Text;

namespace iSynaptic.Commons
{
    public static class DependencyResolverExtensions
    {
        private static Func<IDependencyResolver, ISymbol, Maybe<object>> _ResolveStrategy = null;

        public static T Resolve<T>(this IDependencyResolver resolver)
        {
            return (T)Resolve(resolver, typeof(T));
        }

        public static T Resolve<T>(this IDependencyResolver resolver, string name)
        {
            return (T)Resolve(resolver, typeof(T), name);
        }

        public static T Resolve<T>(this IDependencyResolver resolver, ISymbol<T> symbol)
        {
            return (T) Resolve(resolver, (ISymbol) symbol);
        }

        public static object Resolve(this IDependencyResolver resolver, Type dependencyType)
        {
            return Resolve(resolver, dependencyType, null);
        }

        public static object Resolve(this IDependencyResolver resolver, Type dependencyType, string name)
        {
            return Resolve(resolver, new DependencySymbol(dependencyType, name));
        }

        public static object Resolve(this IDependencyResolver resolver, ISymbol symbol)
        {
            return TryResolve(resolver, symbol).ValueOrDefault();
        }

        public static Maybe<T> TryResolve<T>(this IDependencyResolver resolver)
        {
            return TryResolve(resolver, typeof(T)).Cast<T>();
        }

        public static Maybe<T> TryResolve<T>(this IDependencyResolver resolver, string name)
        {
            return TryResolve(resolver, typeof(T), name).Cast<T>();
        }

        public static Maybe<T> TryResolve<T>(this IDependencyResolver resolver, ISymbol<T> symbol)
        {
            return TryResolve(resolver, (ISymbol) symbol).Cast<T>();
        }

        public static Maybe<object> TryResolve(this IDependencyResolver resolver, Type dependencyType)
        {
            return TryResolve(resolver, dependencyType, null);
        }

        public static Maybe<object> TryResolve(this IDependencyResolver resolver, Type dependencyType, string name)
        {
            return TryResolve(resolver, new DependencySymbol(dependencyType, name));
        }

        private static Maybe<object> TryResolve(IDependencyResolver resolver, ISymbol symbol)
        {
            return ResolveStrategy(resolver, symbol);
        }

        public static void SetResolveStrategy(Func<IDependencyResolver, ISymbol, Maybe<object>> strategy)
        {
            _ResolveStrategy = strategy;
        }

        private static Func<IDependencyResolver, ISymbol, Maybe<object>> ResolveStrategy
        {
            get { return _ResolveStrategy ?? DefaultResolveStrategy; }
        }

        private static Maybe<object> DefaultResolveStrategy(IDependencyResolver resolver, ISymbol symbol)
        {
            Guard.NotNull(resolver, "resolver");
            Guard.NotNull(symbol, "symbol");

            return resolver.TryResolve(symbol);
        }

        private class DependencySymbol : NamedSymbol, IDependencySymbol
        {
            public DependencySymbol(Type dependencyType, string name) : base(name)
            {
                DependencyType = dependencyType;
            }

            public Type DependencyType { get; private set; }
        }
    }
}
