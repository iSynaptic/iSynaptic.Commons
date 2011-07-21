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
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons
{
    [TestFixture]
    public class DependencyResolverTests
    {
        [Test]
        public void Construction_WithNullResolutionStrategy_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DependencyResolver(null));
        }

        [Test]
        public void Resolve_WithAbritraryParameters_CallsResolutionStrategyCorrectly()
        {
            string name = null;
            Type dependencyType = null;

            Func<ISymbol, Maybe<object>> strategy = symbol =>
                                                {
                                                    name = ((IDependencySymbol) symbol).Name;
                                                    dependencyType = ((IDependencySymbol) symbol).DependencyType;
                                                    return Maybe<object>.NoValue;
                                                };

            var resolver = new DependencyResolver(strategy);
            resolver.Resolve(typeof(IDisposable), "Foo");

            Assert.AreEqual("Foo", name);
            Assert.AreEqual(typeof(IDisposable), dependencyType);

        }

        [Test]
        public void Resolve_WithAbritraryParameters_ReturnsResultCorrectly()
        {
            Func<ISymbol, Maybe<object>> strategy = _ => "Baz".ToMaybe<object>();

            var resolver = new DependencyResolver(strategy);
            var result = resolver.Resolve(typeof (IDisposable), "Foo");

            Assert.AreEqual("Baz", result);
        }
    }
}
