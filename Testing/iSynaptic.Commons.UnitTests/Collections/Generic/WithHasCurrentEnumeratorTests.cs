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
using System.Text;
using System.Threading.Tasks;
using iSynaptic.Commons.Linq;
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class WithHasCurrentEnumeratorTests
    {
        [Test]
        public void WithEmptyCollection()
        {
            var source = Enumerable.Empty<int>().GetEnumerator().WithHasCurrent();
            
            Assert.IsFalse(source.MoveNext());
            Assert.IsFalse(source.HasCurrent);
        }

        [Test]
        public void WithSingleValueInCollection()
        {
            var source = new[] {42}.GetEnumerator().WithHasCurrent();

            Assert.IsTrue(source.MoveNext());
            Assert.IsTrue(source.HasCurrent);
            Assert.AreEqual(42, source.Current);

            Assert.IsFalse(source.MoveNext());
            Assert.IsFalse(source.HasCurrent);
        }

        [Test]
        public void WithMultipleValuesInCollection()
        {
            var source = new[] {12, 42, 2001}.GetEnumerator().WithHasCurrent();

            Assert.IsTrue(source.MoveNext());
            Assert.IsTrue(source.HasCurrent);
            Assert.AreEqual(12, source.Current);

            Assert.IsTrue(source.MoveNext());
            Assert.IsTrue(source.HasCurrent);
            Assert.AreEqual(42, source.Current);

            Assert.IsTrue(source.MoveNext());
            Assert.IsTrue(source.HasCurrent);
            Assert.AreEqual(2001, source.Current);

            Assert.IsFalse(source.MoveNext());
            Assert.IsFalse(source.HasCurrent);
        }

        [Test]
        public void HasCurrent_WithoutCallingMoveNext_CallsMoveNext()
        {
            var source = new[] { 42 }.GetEnumerator().WithHasCurrent();

            Assert.IsTrue(source.HasCurrent);
            Assert.AreEqual(42, source.Current);
        }
    }
}
