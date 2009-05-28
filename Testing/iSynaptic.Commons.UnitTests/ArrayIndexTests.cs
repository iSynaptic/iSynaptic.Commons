using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using System.Linq;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class ArrayIndexTests : BaseTestFixture
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
