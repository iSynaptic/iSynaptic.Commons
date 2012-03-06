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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;

namespace iSynaptic.Commons.Collections.Generic
{
    [TestFixture]
    public class ProjectionCollectionTests
    {
        [Test]
        public void ProjectedCollection_IsReadOnly()
        {
            var source = new List<int> { 1, 2, 3 };
            ICollection<int> projection = source.ToProjectedCollection(x => x * 2);

            Assert.IsTrue(projection.IsReadOnly);
            Assert.Throws<NotSupportedException>(() => projection.Add(4));
            Assert.Throws<NotSupportedException>(() => projection.Clear());
            Assert.Throws<NotSupportedException>(() => projection.Remove(1));
        }

        [Test]
        public void ProjectedCollection_IsEnumerable()
        {
            var source = new List<int> { 1, 2, 3 };
            ICollection<int> projection = source.ToProjectedCollection(x => x * 2);

            Assert.IsTrue(projection.SequenceEqual(new[] { 2, 4, 6 }));
            Assert.IsTrue(((IEnumerable)projection).OfType<int>().SequenceEqual(new[] {2, 4, 6}));

        }

        [Test]
        public void ProjectedCollection_Count_ReturnsCorrectly()
        {
            var source = MockRepository.GenerateStub<ICollection<int>>();
            source.Expect(x => x.Count)
                .Return(42);

            ICollection<int> projection = source.ToProjectedCollection(x => x * 2);

            Assert.AreEqual(42, projection.Count);
        }

        [Test]
        public void ProjectedCollection_CopyTo_CopiesDataCorrectly()
        {
            var source = new List<int> { 1, 2, 3 };
            ICollection<int> projection = source.ToProjectedCollection(x => x * 2);

            int[] copy = new int[source.Count];
            projection.CopyTo(copy, 0);

            Assert.IsTrue(copy.SequenceEqual(new[] { 2, 4, 6 }));
        }

        [Test]
        public void ProjectedCollection_Contains_ReturnsCorrectly()
        {
            var source = new List<int> { 1, 2, 3 };
            ICollection<string> projection = source.ToProjectedCollection(x => (x * 2).ToString());

            Assert.IsFalse(projection.Contains("1"));
            Assert.IsTrue(projection.Contains("2"));
            Assert.IsFalse(projection.Contains("3"));
            Assert.IsTrue(projection.Contains("4"));
            Assert.IsFalse(projection.Contains("5"));
            Assert.IsTrue(projection.Contains("6"));

            Assert.IsFalse(projection.Contains(null));
        }
    }
}
