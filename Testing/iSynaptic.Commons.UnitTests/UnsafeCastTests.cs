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
    public class A
    {
        public int Foo { get; set; }
    }

    public class B
    {
        public int Foo { get; set; }
        public int Bar { get; set; }
    }

    public class C
    {
        public int Bar { get; set; }
    }

    [TestFixture]
    public class UnsafeCastTests
    {
        [Test]
        public void UnsafeCast_DelegateOfSameTypeSignature()
        {
            Predicate<string> predicate = x => x.Length % 2 == 0;
            Func<string, bool> func = UnsafeCast<Predicate<string>, Func<string, bool>>.With(predicate);

            Assert.IsTrue(func("Hello!"));
        }

        [Test]
        public void UnsafeCast_ClassToClassOfSameStructure()
        {
            A source = new A {Foo = 42};
            C result = UnsafeCast<A, C>.With(source);

            Assert.AreEqual(42, result.Bar);
        }

        [Test]
        public void UnsafeCast_ClassToClassOfSupersetStructure()
        {
            A source = new A { Foo = 42 };
            B result = UnsafeCast<A, B>.With(source);

            Assert.AreEqual(42, result.Foo);
            Assert.AreEqual(0, result.Bar);
        }
    }
}
