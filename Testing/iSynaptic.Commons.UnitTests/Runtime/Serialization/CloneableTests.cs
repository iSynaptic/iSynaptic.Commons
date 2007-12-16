using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons.UnitTests.Runtime.Serialization
{
    [TestFixture]
    public class CloneableTests
    {
        [Test]
        public void CloneString()
        {
            Assert.IsTrue(Cloneable<string>.CanClone());
            Assert.AreEqual("Testing...", Cloneable<string>.Clone("Testing..."));
        }

        [Test]
        public void CloneInt16()
        {
            Assert.IsTrue(Cloneable<Int16>.CanClone());
            Assert.AreEqual((Int16) (-16), Cloneable<Int16>.Clone(-16));
        }

        [Test]
        public void CloneUInt16()
        {
            Assert.IsTrue(Cloneable<UInt16>.CanClone());
            Assert.AreEqual((UInt16)16, Cloneable<UInt16>.Clone(16));
        }

        [Test]
        public void CloneInt32()
        {
            Assert.IsTrue(Cloneable<Int32>.CanClone());
            Assert.AreEqual((Int32)(-32), Cloneable<Int32>.Clone(-32));
        }

        [Test]
        public void CloneUInt32()
        {
            Assert.IsTrue(Cloneable<UInt32>.CanClone());
            Assert.AreEqual((UInt32)32, Cloneable<UInt32>.Clone(32));
        }

        [Test]
        public void CloneInt64()
        {
            Assert.IsTrue(Cloneable<Int64>.CanClone());
            Assert.AreEqual((Int64)(-64), Cloneable<Int64>.Clone(-64));
        }

        [Test]
        public void CloneUInt64()
        {
            Assert.IsTrue(Cloneable<UInt64>.CanClone());
            Assert.AreEqual((UInt64)64, Cloneable<UInt64>.Clone(64));
        }

        [Test]
        public void CloneByte()
        {
            Assert.IsTrue(Cloneable<Byte>.CanClone());
            Assert.AreEqual((Byte)(8), Cloneable<Byte>.Clone(8));
        }

        [Test]
        public void CloneSByte()
        {
            Assert.IsTrue(Cloneable<SByte>.CanClone());
            Assert.AreEqual((SByte)(-8), Cloneable<SByte>.Clone(-8));
        }

        [Test]
        public void CloneBoolean()
        {
            Assert.IsTrue(Cloneable<Boolean>.CanClone());
            Assert.AreEqual(true, Cloneable<Boolean>.Clone(true));
        }

        [Test]
        public void CloneDouble()
        {
            Assert.IsTrue(Cloneable<Double>.CanClone());
            Assert.AreEqual((Double) 128, Cloneable<Double>.Clone(128));
        }

        [Test]
        public void CloneChar()
        {
            Assert.IsTrue(Cloneable<Char>.CanClone());
            Assert.AreEqual('A', Cloneable<Char>.Clone('A'));
        }

        [Test]
        public void CloneSingle()
        {
            Assert.IsTrue(Cloneable<Single>.CanClone());
            Assert.AreEqual((Single)64, Cloneable<Single>.Clone(64));
        }

        [Test]
        public void CloneGuid()
        {
            Guid id = new Guid("6C31A07E-F633-479b-836B-9479CD7EF92F");

            Assert.IsTrue(Cloneable<Guid>.CanClone());
            Assert.AreEqual(id, Cloneable<Guid>.Clone(id));
        }

        [Test]
        public void CloneDateTime()
        {
            DateTime now = DateTime.Now;

            Assert.IsTrue(Cloneable<DateTime>.CanClone());
            Assert.AreEqual(now, Cloneable<DateTime>.Clone(now));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCloneIntPtr()
        {
            Assert.IsFalse(Cloneable<IntPtr>.CanClone());
            Assert.AreEqual(IntPtr.Zero, Cloneable<IntPtr>.Clone(IntPtr.Zero));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCloneComplexType()
        {
            Assert.IsFalse(Cloneable<NotCloneableStub>.CanClone());
            Assert.IsNull(Cloneable<NotCloneableStub>.Clone(new NotCloneableStub()));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotCloneDerivedComplexType()
        {
            Assert.IsFalse(Cloneable<DerivedNotCloneableStub>.CanClone());
            Assert.IsNull(Cloneable<DerivedNotCloneableStub>.Clone(new DerivedNotCloneableStub()));
        }
    }
}
