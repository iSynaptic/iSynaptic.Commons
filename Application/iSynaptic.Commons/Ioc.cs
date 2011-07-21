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
using System.Diagnostics;

namespace iSynaptic.Commons
{
    public static class Ioc
    {
        private class NullDependencyResolver : IDependencyResolver
        {
            public Maybe<object> TryResolve(ISymbol symbol)
            {
                return Maybe<object>.NoValue;
            }
        }

        private static IDependencyResolver _DependencyResolver = null;

        public static T Resolve<T>()
        {
            return DependencyResolver.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return DependencyResolver.Resolve<T>(name);
        }

        public static object Resolve(Type dependencyType)
        {
            return DependencyResolver.Resolve(dependencyType);
        }

        public static object Resolve(Type dependencyType, string name)
        {
            return DependencyResolver.Resolve(dependencyType, name);
        }

        public static Maybe<T> TryResolve<T>()
        {
            return DependencyResolver.TryResolve<T>();
        }

        public static Maybe<T> TryResolve<T>(string name)
        {
            return DependencyResolver.TryResolve<T>(name);
        }

        public static Maybe<object> TryResolve(Type dependencyType)
        {
            return DependencyResolver.TryResolve(dependencyType);
        }

        public static Maybe<object> TryResolve(Type dependencyType, string name)
        {
            return DependencyResolver.TryResolve(dependencyType, name);
        }

        public static void SetDependencyResolver(IDependencyResolver resolver)
        {
            DependencyResolver = resolver;
        }

        private static IDependencyResolver DependencyResolver
        {
            get { return _DependencyResolver ?? new NullDependencyResolver(); }
            set { _DependencyResolver = value; }
        }
    }
}
