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
using NUnit.Framework;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class CollectionExtensionsTests
    {
        [Test]
        public void RemoveWithNull()
        {
            ICollection<int> col = null;
            Assert.Throws<ArgumentNullException>(() => col.Remove(1, 2, 3));
        }

        [Test]
        public void RemoveWithNullItems()
        {
            ICollection<int> col = new List<int> { 1, 2, 3 };

            col.Remove((int[])null);
            Assert.IsTrue(col.SequenceEqual(new [] { 1, 2, 3 }));
        }

        [Test]
        public void RemoveWithEmptyItems()
        {
            ICollection<int> col = new List<int> { 1, 2, 3 };

            col.Remove(new int[] { });
            Assert.IsTrue(col.SequenceEqual(new [] { 1, 2, 3 }));
        }

        [Test]
        public void RemoveItems()
        {
            ICollection<int> col = new List<int> { 1, 2, 3, 4, 5 };

            col.Remove(new int[] { 2, 4 });
            Assert.IsTrue(col.SequenceEqual(new [] { 1, 3, 5 }));
        }

        [Test]
        public void RemoveAll()
        {
            ICollection<int> col = new List<int> { 1, 2, 3, 4, 5 };

            col.RemoveAll(x => x % 2 == 0);
            Assert.IsTrue(col.SequenceEqual(new [] { 1, 3, 5 }));
        }

        [Test]
        public void RemoveAllWithNullCollection()
        {
            ICollection<int> col = null;
            Assert.Throws<ArgumentNullException>(() => col.RemoveAll(x => true));
        
        }

        [Test]
        public void RemoveAllWithNullPredicate()
        {
            ICollection<int> col = new List<int> { 1, 2, 3 };
            Assert.Throws<ArgumentNullException>(() => col.RemoveAll(null));
        }
    }
}
