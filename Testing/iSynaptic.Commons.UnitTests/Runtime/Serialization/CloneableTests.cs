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
            Assert.AreEqual((Int16)(-16), Cloneable<Int16>.Clone(-16));
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
            Assert.AreEqual((Double)128, Cloneable<Double>.Clone(128));
        }

        [Test]
        public void CloneDecimal()
        {
            Assert.IsTrue(Cloneable<decimal>.CanClone());
            Assert.AreEqual((decimal)128, Cloneable<decimal>.Clone(128));
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
        public void CloneTimeSpan()
        {
            TimeSpan source = TimeSpan.FromMinutes(5);

            Assert.IsTrue(Cloneable<TimeSpan>.CanClone());
            Assert.AreEqual(source, Cloneable<TimeSpan>.Clone(source));
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

        [Test]
        public void CloneComplexType()
        {
            CloneableStub stub = new CloneableStub
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                BirthDate = DateTime.Now,
                YearsOfService = 25
            };

            CloneableStub clonedStub = Cloneable<CloneableStub>.Clone(stub);

            Assert.IsTrue(Cloneable<CloneableStub>.CanClone());
            Assert.IsNotNull(clonedStub);
            Assert.IsFalse(object.ReferenceEquals(stub, clonedStub));

            Assert.IsTrue(clonedStub.Equals(stub));
        }

        [Test]
        public void CloneDerivedComplexType()
        {
            DerivedCloneableStub stub = new DerivedCloneableStub
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                BirthDate = DateTime.Now,
                YearsOfService = 25,
                DerivedName = "DerivedName"
            };

            DerivedCloneableStub clonedStub = Cloneable<DerivedCloneableStub>.Clone(stub);

            Assert.IsTrue(Cloneable<DerivedCloneableStub>.CanClone());
            Assert.IsNotNull(clonedStub);
            Assert.IsFalse(object.ReferenceEquals(stub, clonedStub));

            Assert.IsTrue(stub.Equals(clonedStub));
        }

        [Test]
        public void CloneArray()
        {
            Guid idOne = Guid.NewGuid();
            Guid idTwo = Guid.NewGuid();

            DateTime birthDateOne = DateTime.Now.AddDays(-1);
            DateTime birthDateTwo = DateTime.Now.AddDays(-2);

            CloneableStub[] stubs = 
            {
                new CloneableStub { Id = idOne, BirthDate = birthDateOne, Name = "Stub 1", YearsOfService = 1},
                new CloneableStub { Id = idTwo, BirthDate = birthDateTwo, Name = "Stub 2", YearsOfService = 2}
            };

            var clones = Cloneable<CloneableStub[]>.Clone(stubs);

            Assert.IsTrue(Cloneable<CloneableStub[]>.CanClone());

            Assert.IsFalse(object.ReferenceEquals(stubs, clones));
            Assert.AreEqual(stubs.Length, clones.Length);

            Assert.IsFalse(object.ReferenceEquals(stubs[0], clones[0]));
            Assert.IsFalse(object.ReferenceEquals(stubs[1], clones[1]));

            Assert.IsTrue(stubs[0].Equals(clones[0]));
            Assert.IsTrue(stubs[1].Equals(clones[1]));
        }

        [Test]
        public void CloneMultidimensionalArray()
        {
            Guid idOne = Guid.NewGuid();
            Guid idTwo = Guid.NewGuid();

            DateTime birthDateOne = DateTime.Now.AddDays(-1);
            DateTime birthDateTwo = DateTime.Now.AddDays(-2);

            CloneableStub[,] stubs = 
            {
                {
                new CloneableStub { Id = idOne, BirthDate = birthDateOne, Name = "Stub 1", YearsOfService = 1},
                new CloneableStub { Id = idTwo, BirthDate = birthDateTwo, Name = "Stub 2", YearsOfService = 2}
                }
            };

            var clones = Cloneable<CloneableStub[,]>.Clone(stubs);

            Assert.IsFalse(object.ReferenceEquals(stubs, clones));
            Assert.AreEqual(stubs.Length, clones.Length);

            Assert.IsFalse(object.ReferenceEquals(stubs[0, 0], clones[0, 0]));
            Assert.IsFalse(object.ReferenceEquals(stubs[0, 1], clones[0, 1]));

            Assert.IsTrue(stubs[0, 0].Equals(clones[0, 0]));
            Assert.IsTrue(stubs[0, 1].Equals(clones[0, 1]));
        }

        [Test]
        public void CloneJaggedArray()
        {
            Guid idOne = Guid.NewGuid();
            Guid idTwo = Guid.NewGuid();

            DateTime birthDateOne = DateTime.Now.AddDays(-1);
            DateTime birthDateTwo = DateTime.Now.AddDays(-2);

            CloneableStub[][] stubs = 
            {
                new CloneableStub[] {
                    new CloneableStub { Id = idOne, BirthDate = birthDateOne, Name = "Stub 1", YearsOfService = 1},
                    new CloneableStub { Id = idTwo, BirthDate = birthDateTwo, Name = "Stub 2", YearsOfService = 2}
                }
            };

            var clones = Cloneable<CloneableStub[][]>.Clone(stubs);

            Assert.IsTrue(Cloneable<CloneableStub[][]>.CanClone());

            Assert.IsFalse(object.ReferenceEquals(stubs, clones));
            Assert.AreEqual(stubs.Length, clones.Length);

            Assert.IsFalse(object.ReferenceEquals(stubs[0][0], clones[0][0]));
            Assert.IsFalse(object.ReferenceEquals(stubs[0][1], clones[0][1]));

            Assert.IsTrue(stubs[0][0].Equals(clones[0][0]));
            Assert.IsTrue(stubs[0][1].Equals(clones[0][1]));

        }
    }
}
