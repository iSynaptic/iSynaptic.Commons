using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.UnitTests
{
    [TestFixture]
    public class ReadableQualifierTests : BaseTestFixture
    {
        [Test]
        public void NullGetHandler()
        {
            Assert.Throws<ArgumentNullException>(() => new ReadableQualifier<int, int>(null));
        }

        [Test]
        public void IndexerReturnsCorrectly()
        {
            var qualifier = new ReadableQualifier<int, int>(i => i * 2);

            Assert.AreEqual(10, qualifier[5]);
            Assert.AreEqual(0, qualifier[0]);
            Assert.AreEqual(6, qualifier[3]);
        }

        [Test]
        public void KnownQualifiersReturnsNull()
        {
            var qualifier = new ReadableQualifier<int, int>(i => i);
            Assert.IsNull(qualifier.KnownQualifiers);
        }

        [Test]
        public void KnownQualifiersReturnsCorrectly()
        {
            var qualifier = new ReadableQualifier<int, int>(i => i, () => new int[] { });
            
            Assert.IsNotNull(qualifier.KnownQualifiers);
            Assert.AreEqual(0, qualifier.KnownQualifiers.Length);
        }
    }
}
