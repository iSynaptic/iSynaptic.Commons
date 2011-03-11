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

        [Test]
        public void Equals_WithSameUnderlyingObject_ReturnsTrue()
        {
            string target = "Hello, World!";

            var ref1 = WeakReference<string>.Create(target);
            var ref2 = WeakReference<string>.Create(target);

            Assert.IsTrue(ref1.Equals(ref2));
        }

        [Test]
        public void Equals_WithDifferentUnderlyingObject_ReturnsFalse()
        {
            var ref1 = WeakReference<string>.Create("Hello");
            var ref2 = WeakReference<string>.Create("World!");

            Assert.IsFalse(ref1.Equals(ref2));
        }

        [Test]
        public void Equals_WithNull_ReturnsTrue()
        {
            var ref1 = WeakReference<string>.Create(null);
            var ref2 = WeakReference<string>.Create(null);

            Assert.IsTrue(ref1.Equals(ref2));
        }
    }
}
