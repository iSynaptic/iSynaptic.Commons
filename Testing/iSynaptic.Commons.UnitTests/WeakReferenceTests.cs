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
    public class WeakReferenceTests
    {
        [Test]
        public void Target_ReturnsCorrectly()
        {
            object foo = new object();
            var weakRef = WeakReference<object>.Create(foo);

            Assert.IsTrue(ReferenceEquals(foo, weakRef.Target));
        }

        [Test]
        public void Create_WithNull_ReturnsWeakNullReference()
        {
            var weakRef = WeakReference<object>.Create(null);
            Assert.IsTrue(ReferenceEquals(WeakReference<object>.Null, weakRef));
        }

        [Test]
        public void IsAlive_ForWeakNullReference_ReturnsTrue()
        {
            Assert.IsTrue(WeakReference<object>.Null.IsAlive);
        }

        [Test]
        public void Equals_WithSameUnderlyingObject_ReturnsTrue()
        {
            string target = "Hello, World!";

            var ref1 = WeakReference<string>.Create(target);
            var ref2 = WeakReference<string>.Create(target);

            Assert.IsTrue(ref1.Equals(ref2));
        }

        [Test]
        public void Equals_WithDifferentUnderlyingObject_ReturnsFalse()
        {
            var ref1 = WeakReference<string>.Create("Hello");
            var ref2 = WeakReference<string>.Create("World!");

            Assert.IsFalse(ref1.Equals(ref2));
        }

        [Test]
        public void Equals_WithNull_ReturnsTrue()
        {
            var ref1 = WeakReference<string>.Create(null);
            var ref2 = WeakReference<string>.Create(null);

            Assert.IsTrue(ref1.Equals(ref2));
        }
    }
}
