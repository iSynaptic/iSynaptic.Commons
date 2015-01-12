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
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Data
{
    [TestFixture]
    public class ExodataResolverTests
    {
        [Test]
        public void TryResolve_WithNoBindings_ReturnsNoValue()
        {
            var symbol = new Symbol();
            var source = new ExodataBindingModule();

            var resolver = new ExodataResolver(new[] { source });

            Maybe<int> value = resolver.TryResolve(ExodataRequest.Create<int, object, object>(symbol, Maybe<object>.NoValue, Maybe<object>.NoValue, null));

            Assert.AreEqual(Maybe<int>.NoValue, value);
        }

        [Test]
        public void TryResolve_WithAmbiguousBindingSelection_ThrowsAmbiguousExodataBindingsException()
        {
            var symbol = new Symbol();
            var source = new ExodataBindingModule();
            source.Bind(symbol, 42);
            source.Bind(symbol, 42);

            var resolver = new ExodataResolver(new[] { source });

            Maybe<int> result = resolver.TryResolve(ExodataRequest.Create<int, object, object>(symbol, Maybe<object>.NoValue, Maybe<object>.NoValue, null));

            var exception = Assert.Throws<AmbiguousExodataBindingsException>(() => { var x = result.HasValue; });

            Assert.IsNotNull(exception);
            Assert.AreEqual(2, exception.Bindings.Count());
        }

        [Test]
        public void TryResolve_WithNonAmbiguousBindingSelection_YieldsValue()
        {
            var symbol = new Symbol();
            var source = new ExodataBindingModule();
            source.Bind(symbol, 42);

            var resolver = new ExodataResolver(new[] { source });

            Maybe<int> result = resolver.TryResolve(ExodataRequest.Create<int, object, object>(symbol, Maybe<object>.NoValue, Maybe<object>.NoValue, null));

            Assert.AreEqual(42, result.Value);
        }
    }
}
