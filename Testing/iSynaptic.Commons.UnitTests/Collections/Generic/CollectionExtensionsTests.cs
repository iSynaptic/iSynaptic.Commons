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
            var col = new List<int> { 1, 2, 3 };

            col.Remove((int[])null);
            Assert.IsTrue(col.SequenceEqual(new int[] { 1, 2, 3 }));
        }

        [Test]
        public void RemoveWithEmptyItems()
        {
            var col = new List<int> { 1, 2, 3 };

            col.Remove(new int[] { });
            Assert.IsTrue(col.SequenceEqual(new int[] { 1, 2, 3 }));
        }

        [Test]
        public void RemoveItems()
        {
            var col = new List<int> { 1, 2, 3, 4, 5 };

            col.Remove(new int[] { 2, 4 });
            Assert.IsTrue(col.SequenceEqual(new int[] { 1, 3, 5 }));
        }
    }
}
