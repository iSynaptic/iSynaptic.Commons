using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.UnitTests.Extensions
{
    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        public void WithIndex()
        {
            int[] items = { 1, 3, 5, 7, 9 };

            int index = 0;
            foreach (var item in items.WithIndex())
            {
                Assert.AreEqual(index, item.Index);
                Assert.AreEqual(items[index], item.Value);

                index++;
            }
        }

        [Test]
        public void LookAheadEnumerable()
        {
            int[] items = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            using (var enumerator = items.AsLookAheadable().GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(1, enumerator.Current.Value);
                Assert.AreEqual(2, enumerator.Current.LookAhead(0));

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(2, enumerator.Current.Value);
                Assert.AreEqual(3, enumerator.Current.LookAhead(0));
                Assert.AreEqual(5, enumerator.Current.LookAhead(2));
                Assert.AreEqual(4, enumerator.Current.LookAhead(1));

                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreEqual(3, enumerator.Current.Value);
                Assert.AreEqual(4, enumerator.Current.LookAhead(0));
            }
        }
    }
}
