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

namespace iSynaptic.Commons
{
    [TestFixture]
    public class ArrayIndexTests
    {
        [Test]
        public void NullArray()
        {
            Assert.Throws<ArgumentNullException>(() => new ArrayIndex(null));
        }

        [Test]
        public void EmptyArray()
        {
            var idx = new ArrayIndex(new int[] { });
            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 0 }));

            Assert.IsFalse(idx.CanIncrement());
            Assert.Throws<IndexOutOfRangeException>(() => { idx.Increment(); });

            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 0 }));
        }

        [Test]
        public void SimpleArray()
        {
            var idx = new ArrayIndex(new int[] { 1, 2, 3, 4, 5 });
            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 0 }));

            Assert.IsTrue(idx.CanIncrement());
            idx.Increment();

            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 1 }));

            idx.Increment();
            idx.Increment();

            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 3 }));

            idx.Increment();
            Assert.Throws<IndexOutOfRangeException>(() => { idx.Increment(); });

            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 4 }));
        }

        [Test]
        public void RankedArray()
        {
            var idx = new ArrayIndex(new int[,] { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 4, 4 }, { 5, 5 } });
            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 0, 0 }));

            Assert.IsTrue(idx.CanIncrement());
            idx.Increment();

            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 0, 1 }));

            idx.Increment();
            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 1, 0 }));

            idx.Increment();
            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 1, 1 }));

            Assert.Throws<IndexOutOfRangeException>(() => { idx.Increment(10); });

            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 1, 1 }));
        }

        [Test]
        public void IncrementMultiple()
        {
            var idx = new ArrayIndex(new int[] { 1, 2, 3, 4, 5 });

            idx.Increment(3);
            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 3 }));

            Assert.Throws<IndexOutOfRangeException>(() => { idx.Increment(5); });

            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 3 }));
        }

        [Test]
        public void Reset()
        {
            var idx = new ArrayIndex(new int[] { 1, 2, 3, 4, 5 });

            idx.Increment(3);
            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 3 }));

            idx.Reset();
            Assert.IsTrue(idx.Index.SequenceEqual(new int[] { 0 }));
        }
    }
}
