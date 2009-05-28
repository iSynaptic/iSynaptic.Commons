using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class ReadWriteQualifierTests : BaseTestFixture
    {
        [Test]
        public void NullGetHandler()
        {
            AssertThrows<ArgumentNullException>(() => new ReadWriteQualifier<int, int>(null, (q, v) => { }));
        }

        [Test]
        public void NullSetHandler()
        {
            AssertThrows<ArgumentNullException>(() => new ReadWriteQualifier<int, int>(i => i, null));
        }

        [Test]
        public void IndexerReturnsCorrectly()
        {
            var qualifier = new ReadWriteQualifier<int, int>(i => i * 2, (q, v) => { });

            Assert.AreEqual(10, qualifier[5]);
            Assert.AreEqual(0, qualifier[0]);
            Assert.AreEqual(6, qualifier[3]);
        }

        [Test]
        public void IndexerSetsCorrectly()
        {
            bool setExecuted = false;

            var qualifier = new ReadWriteQualifier<int, int>(i => i, (q, v) =>
            {
                Assert.AreEqual(1, q);
                Assert.AreEqual(5, v);

                setExecuted = true;
            });

            qualifier[1] = 5;
            Assert.IsTrue(setExecuted);
        }

        [Test]
        public void KnownQualifiersReturnsNull()
        {
            var qualifier = new ReadWriteQualifier<int, int>(i => i, (q, v) => { });
            Assert.IsNull(qualifier.KnownQualifiers);
        }

        [Test]
        public void KnownQualifiersReturnsCorrectly()
        {
            var qualifier = new ReadWriteQualifier<int, int>(i => i, (q, v) => { }, () => new int[] { });

            Assert.IsNotNull(qualifier.KnownQualifiers);
            Assert.AreEqual(0, qualifier.KnownQualifiers.Length);
        }
    }
}
