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
    }
}
