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

namespace iSynaptic.Commons.Reflection
{
    [TestFixture]
    public class ReflectionExtensionsTests
    {
        [Test]
        public void GetAttributesOfType_WithNullProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(
                () => ReflectionExtensions.GetAttributesOfType<TestFixtureAttribute>(null));
        }

        [Test]
        public void GetAttributesOfType_AgainstThisTestFixture_ReturnsTestFixtureAttribute()
        {
            var attribute = GetType()
                .GetAttributesOfType<TestFixtureAttribute>()
                .Single();

            Assert.IsNotNull(attribute);
        }

        [Test]
        public void GetFieldsDeeply_WithNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReflectionExtensions.GetFieldsDeeply(null));
        }

        [Test]
        public void GetFieldsDeeply_WorksRecursively()
        {
            var fields = typeof (FurtherDerived).GetFieldsDeeply()
                .Select(x => x.Name)
                .OrderBy(x => x);

            Assert.IsTrue(fields.SequenceEqual(new []{"Bar", "Baz", "Foo", "Quux"}));
        }

        [Test]
        public void GetFieldsDeeply_WithFilter_WorksRecursively()
        {
            var fields = typeof(FurtherDerived).GetFieldsDeeply(x => !x.Name.StartsWith("B"))
                .Select(x => x.Name)
                .OrderBy(x => x);

            Assert.IsTrue(fields.SequenceEqual(new[] { "Foo", "Quux" }));
        }

        [Test]
        public void DoesImplementType_SupportsOpenGenerics()
        {
            var openType = typeof (Maybe<>);

            var candidate = typeof (Maybe<Int32>);
            Assert.IsTrue(candidate.DoesImplementType(openType));
        }

        private class Base
        {
            public Guid Quux = Guid.Empty;
        }

        private class Derived : Base
        {
            protected int Baz = 42;
            public DateTime Bar = SystemClock.UtcNow;
       }

        private class FurtherDerived : Derived
        {
            private string Foo = "Foo";

            public FurtherDerived()
            {
                Console.WriteLine(Foo);
            }
        }
    }
}
