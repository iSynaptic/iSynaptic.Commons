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
            Assert.IsTrue(Cloneable<string>.CanShallowClone());

            Assert.AreEqual("Testing...", Cloneable<string>.Clone("Testing..."));
            Assert.AreEqual("Testing...", Cloneable<string>.ShallowClone("Testing..."));
        }

        [Test]
        public void CloneInt16()
        {
            Assert.IsTrue(Cloneable<Int16>.CanClone());
            Assert.IsTrue(Cloneable<Int16>.CanShallowClone());

            Assert.AreEqual((Int16)(-16), Cloneable<Int16>.Clone(-16));
            Assert.AreEqual((Int16)(-16), Cloneable<Int16>.ShallowClone(-16));
        }

        [Test]
        public void CloneUInt16()
        {
            Assert.IsTrue(Cloneable<UInt16>.CanClone());
            Assert.IsTrue(Cloneable<UInt16>.CanShallowClone());

            Assert.AreEqual((UInt16)16, Cloneable<UInt16>.Clone(16));
            Assert.AreEqual((UInt16)16, Cloneable<UInt16>.ShallowClone(16));
        }

        [Test]
        public void CloneInt32()
        {
            Assert.IsTrue(Cloneable<Int32>.CanClone());
            Assert.IsTrue(Cloneable<Int32>.CanShallowClone());

            Assert.AreEqual((Int32)(-32), Cloneable<Int32>.Clone(-32));
            Assert.AreEqual((Int32)(-32), Cloneable<Int32>.ShallowClone(-32));
        }

        [Test]
        public void CloneUInt32()
        {
            Assert.IsTrue(Cloneable<UInt32>.CanClone());
            Assert.IsTrue(Cloneable<UInt32>.CanShallowClone());

            Assert.AreEqual((UInt32)32, Cloneable<UInt32>.Clone(32));
            Assert.AreEqual((UInt32)32, Cloneable<UInt32>.ShallowClone(32));
        }

        [Test]
        public void CloneInt64()
        {
            Assert.IsTrue(Cloneable<Int64>.CanClone());
            Assert.IsTrue(Cloneable<Int64>.CanShallowClone());

            Assert.AreEqual((Int64)(-64), Cloneable<Int64>.Clone(-64));
            Assert.AreEqual((Int64)(-64), Cloneable<Int64>.ShallowClone(-64));
        }

        [Test]
        public void CloneUInt64()
        {
            Assert.IsTrue(Cloneable<UInt64>.CanClone());
            Assert.IsTrue(Cloneable<UInt64>.CanShallowClone());

            Assert.AreEqual((UInt64)64, Cloneable<UInt64>.Clone(64));
            Assert.AreEqual((UInt64)64, Cloneable<UInt64>.ShallowClone(64));
        }

        [Test]
        public void CloneByte()
        {
            Assert.IsTrue(Cloneable<Byte>.CanClone());
            Assert.IsTrue(Cloneable<Byte>.CanShallowClone());

            Assert.AreEqual((Byte)(8), Cloneable<Byte>.Clone(8));
            Assert.AreEqual((Byte)(8), Cloneable<Byte>.ShallowClone(8));
        }

        [Test]
        public void CloneSByte()
        {
            Assert.IsTrue(Cloneable<SByte>.CanClone());
            Assert.IsTrue(Cloneable<SByte>.CanShallowClone());

            Assert.AreEqual((SByte)(-8), Cloneable<SByte>.Clone(-8));
            Assert.AreEqual((SByte)(-8), Cloneable<SByte>.ShallowClone(-8));
        }

        [Test]
        public void CloneBoolean()
        {
            Assert.IsTrue(Cloneable<Boolean>.CanClone());
            Assert.IsTrue(Cloneable<Boolean>.CanShallowClone());

            Assert.AreEqual(true, Cloneable<Boolean>.Clone(true));
            Assert.AreEqual(true, Cloneable<Boolean>.ShallowClone(true));
        }

        [Test]
        public void CloneDouble()
        {
            Assert.IsTrue(Cloneable<Double>.CanClone());
            Assert.IsTrue(Cloneable<Double>.CanShallowClone());

            Assert.AreEqual((Double)128, Cloneable<Double>.Clone(128));
            Assert.AreEqual((Double)128, Cloneable<Double>.ShallowClone(128));
        }

        [Test]
        public void CloneDecimal()
        {
            Assert.IsTrue(Cloneable<decimal>.CanClone());
            Assert.IsTrue(Cloneable<decimal>.CanShallowClone());

            Assert.AreEqual((decimal)128, Cloneable<decimal>.Clone(128));
            Assert.AreEqual((decimal)128, Cloneable<decimal>.ShallowClone(128));
        }

        [Test]
        public void CloneChar()
        {
            Assert.IsTrue(Cloneable<Char>.CanClone());
            Assert.IsTrue(Cloneable<Char>.CanShallowClone());

            Assert.AreEqual('A', Cloneable<Char>.Clone('A'));
            Assert.AreEqual('A', Cloneable<Char>.ShallowClone('A'));
        }

        [Test]
        public void CloneSingle()
        {
            Assert.IsTrue(Cloneable<Single>.CanClone());
            Assert.IsTrue(Cloneable<Single>.CanShallowClone());

            Assert.AreEqual((Single)64, Cloneable<Single>.Clone(64));
            Assert.AreEqual((Single)64, Cloneable<Single>.ShallowClone(64));
        }

        [Test]
        public void CloneGuid()
        {
            Guid id = new Guid("6C31A07E-F633-479b-836B-9479CD7EF92F");

            Assert.IsTrue(Cloneable<Guid>.CanClone());
            Assert.IsTrue(Cloneable<Guid>.CanShallowClone());

            Assert.AreEqual(id, Cloneable<Guid>.Clone(id));
            Assert.AreEqual(id, Cloneable<Guid>.ShallowClone(id));
        }

        [Test]
        public void CloneDateTime()
        {
            DateTime now = DateTime.Now;

            Assert.IsTrue(Cloneable<DateTime>.CanClone());
            Assert.IsTrue(Cloneable<DateTime>.CanShallowClone());

            Assert.AreEqual(now, Cloneable<DateTime>.Clone(now));
            Assert.AreEqual(now, Cloneable<DateTime>.ShallowClone(now));
        }

        [Test]
        public void CloneTimeSpan()
        {
            TimeSpan source = TimeSpan.FromMinutes(5);

            Assert.IsTrue(Cloneable<TimeSpan>.CanClone());
            Assert.IsTrue(Cloneable<TimeSpan>.CanShallowClone());

            Assert.AreEqual(source, Cloneable<TimeSpan>.Clone(source));
            Assert.AreEqual(source, Cloneable<TimeSpan>.ShallowClone(source));
        }

        [Test]
        public void CannotCloneIntPtr()
        {
            Assert.IsFalse(Cloneable<IntPtr>.CanClone());
            Assert.IsFalse(Cloneable<IntPtr>.CanShallowClone());

            try
            {
                Assert.AreEqual(IntPtr.Zero, Cloneable<IntPtr>.Clone(IntPtr.Zero));
                Assert.Fail("Exception was not thrown.");
            }
            catch (InvalidOperationException) { }

            try
            {
                Assert.AreEqual(IntPtr.Zero, Cloneable<IntPtr>.ShallowClone(IntPtr.Zero));
                Assert.Fail("Exception was not thrown.");
            }
            catch (InvalidOperationException) { }
        }

        [Test]
        public void CannotCloneComplexType()
        {
            Assert.IsFalse(Cloneable<NotCloneableStub>.CanClone());
            Assert.IsFalse(Cloneable<NotCloneableStub>.CanShallowClone());

            try
            {
                Assert.IsNull(Cloneable<NotCloneableStub>.Clone(new NotCloneableStub()));
                Assert.Fail("Exception was not thrown.");
            }
            catch (InvalidOperationException) { }

            try
            {
                Assert.IsNull(Cloneable<NotCloneableStub>.ShallowClone(new NotCloneableStub()));
                Assert.Fail("Exception was not thrown.");
            }
            catch (InvalidOperationException) { }
        }

        [Test]
        public void CannotCloneDerivedComplexType()
        {
            Assert.IsFalse(Cloneable<DerivedNotCloneableStub>.CanClone());
            Assert.IsFalse(Cloneable<DerivedNotCloneableStub>.CanShallowClone());

            try
            {
                Assert.IsNull(Cloneable<DerivedNotCloneableStub>.Clone(new DerivedNotCloneableStub()));
                Assert.Fail("Exception was not thrown.");
            }
            catch (InvalidOperationException) { }

            try
            {
                Assert.IsNull(Cloneable<DerivedNotCloneableStub>.ShallowClone(new DerivedNotCloneableStub()));
                Assert.Fail("Exception was not thrown.");
            }
            catch (InvalidOperationException) { }
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
            Assert.IsTrue(Cloneable<CloneableStub>.CanShallowClone());

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
            Assert.IsTrue(Cloneable<DerivedCloneableStub>.CanShallowClone());

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

            CloneableStub stubOne = new CloneableStub { Id = idOne, BirthDate = birthDateOne, Name = "Stub 1", YearsOfService = 1 };

            CloneableStub[] stubs = 
            {
                stubOne,
                stubOne,
                new CloneableStub { Id = idTwo, BirthDate = birthDateTwo, Name = "Stub 2", YearsOfService = 2}
            };

            var clones = Cloneable<CloneableStub[]>.Clone(stubs);

            Assert.IsTrue(Cloneable<CloneableStub[]>.CanClone());
            Assert.IsTrue(Cloneable<CloneableStub[]>.CanShallowClone());

            Assert.IsFalse(object.ReferenceEquals(stubs, clones));
            Assert.AreEqual(stubs.Length, clones.Length);

            Assert.IsFalse(object.ReferenceEquals(stubs[0], clones[0]));
            Assert.IsFalse(object.ReferenceEquals(stubs[1], clones[1]));
            Assert.IsFalse(object.ReferenceEquals(stubs[2], clones[2]));

            Assert.IsTrue(stubs[0].Equals(clones[0]));
            Assert.IsTrue(stubs[1].Equals(clones[1]));
            Assert.IsTrue(stubs[2].Equals(clones[2]));

            Assert.IsTrue(object.ReferenceEquals(clones[0], clones[1]));
        }

        [Test]
        public void CloneMultidimensionalArray()
        {
            Guid idOne = Guid.NewGuid();
            Guid idTwo = Guid.NewGuid();

            DateTime birthDateOne = DateTime.Now.AddDays(-1);
            DateTime birthDateTwo = DateTime.Now.AddDays(-2);

            CloneableStub stubOne = new CloneableStub { Id = idOne, BirthDate = birthDateOne, Name = "Stub 1", YearsOfService = 1 };

            CloneableStub[,] stubs = 
            {
                {
                    stubOne,
                    stubOne,
                    new CloneableStub { Id = idTwo, BirthDate = birthDateTwo, Name = "Stub 2", YearsOfService = 2}
                }
            };

            var clones = Cloneable<CloneableStub[,]>.Clone(stubs);

            Assert.IsFalse(object.ReferenceEquals(stubs, clones));
            Assert.AreEqual(stubs.Length, clones.Length);

            Assert.IsFalse(object.ReferenceEquals(stubs[0, 0], clones[0, 0]));
            Assert.IsFalse(object.ReferenceEquals(stubs[0, 1], clones[0, 1]));
            Assert.IsFalse(object.ReferenceEquals(stubs[0, 2], clones[0, 2]));

            Assert.IsTrue(stubs[0, 0].Equals(clones[0, 0]));
            Assert.IsTrue(stubs[0, 1].Equals(clones[0, 1]));
            Assert.IsTrue(stubs[0, 2].Equals(clones[0, 2]));

            Assert.IsTrue(object.ReferenceEquals(clones[0, 0], clones[0, 1]));
        }

        [Test]
        public void CloneJaggedArray()
        {
            Guid idOne = Guid.NewGuid();
            Guid idTwo = Guid.NewGuid();

            DateTime birthDateOne = DateTime.Now.AddDays(-1);
            DateTime birthDateTwo = DateTime.Now.AddDays(-2);

            CloneableStub stubOne = new CloneableStub { Id = idOne, BirthDate = birthDateOne, Name = "Stub 1", YearsOfService = 1 };

            CloneableStub[][] stubs = 
            {
                new CloneableStub[] {
                    stubOne,
                    stubOne,
                    new CloneableStub { Id = idTwo, BirthDate = birthDateTwo, Name = "Stub 2", YearsOfService = 2}
                }
            };

            var clones = Cloneable<CloneableStub[][]>.Clone(stubs);

            Assert.IsTrue(Cloneable<CloneableStub[][]>.CanClone());
            Assert.IsTrue(Cloneable<CloneableStub[][]>.CanShallowClone());

            Assert.IsFalse(object.ReferenceEquals(stubs, clones));
            Assert.AreEqual(stubs.Length, clones.Length);

            Assert.IsFalse(object.ReferenceEquals(stubs[0][0], clones[0][0]));
            Assert.IsFalse(object.ReferenceEquals(stubs[0][1], clones[0][1]));
            Assert.IsFalse(object.ReferenceEquals(stubs[0][2], clones[0][2]));

            Assert.IsTrue(stubs[0][0].Equals(clones[0][0]));
            Assert.IsTrue(stubs[0][1].Equals(clones[0][1]));
            Assert.IsTrue(stubs[0][2].Equals(clones[0][2]));

            Assert.IsTrue(object.ReferenceEquals(clones[0][0], clones[0][1]));
        }

        [Test]
        public void CloneSelfReferencing()
        {
            Guid id = Guid.NewGuid();
            DateTime birthDate = DateTime.Now.AddDays(-1);

            var stub = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };
            stub.FirstChild = stub;
            stub.SecondChild = stub;

            var clone = Cloneable<CloneableStub>.Clone(stub);

            Assert.IsFalse(object.ReferenceEquals(stub, clone));
            Assert.IsFalse(object.ReferenceEquals(stub.FirstChild, clone.FirstChild));
            Assert.IsFalse(object.ReferenceEquals(stub.SecondChild, clone.SecondChild));

            Assert.IsTrue(object.ReferenceEquals(clone, clone.FirstChild));
            Assert.IsTrue(object.ReferenceEquals(clone, clone.SecondChild));
        }

        [Test]
        public void CloneBidirectionalReferences()
        {
            Guid id = Guid.NewGuid();
            DateTime birthDate = DateTime.Now.AddDays(-1);

            var parent = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };
            var child = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };

            parent.FirstChild = child;
            parent.SecondChild = child;

            child.FirstChild = parent;
            child.SecondChild = parent;

            var clonedParent = Cloneable<CloneableStub>.Clone(parent);

            Assert.IsFalse(object.ReferenceEquals(parent, clonedParent));

            Assert.IsFalse(object.ReferenceEquals(parent.FirstChild, clonedParent.FirstChild));
            Assert.IsFalse(object.ReferenceEquals(parent.SecondChild, clonedParent.SecondChild));

            var clonedChild = clonedParent.FirstChild;

            Assert.IsTrue(object.ReferenceEquals(clonedChild.FirstChild, clonedParent));
            Assert.IsTrue(object.ReferenceEquals(clonedChild.SecondChild, clonedParent));
        }

        [Test]
        public void CloneMultipleReferencesToSameArray()
        {
            Guid id = Guid.NewGuid();
            DateTime birthDate = DateTime.Now.AddDays(-1);

            var child = new CloneableStub[]
            {
                new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 },
                new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 }
            };

            var source = new CloneableStub[][]
            {
                child,
                child
            };

            var clone = Cloneable<CloneableStub[][]>.Clone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(clone[0], child));
            Assert.IsFalse(object.ReferenceEquals(clone[1], child));

            Assert.IsTrue(object.ReferenceEquals(clone[0], clone[1]));
        }

        [Test]
        public void ShallowCloneComplexType()
        {
            Guid id = Guid.NewGuid();
            DateTime birthDate = DateTime.Now.AddDays(-1);

            var parent = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };
            var child = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };

            parent.FirstChild = child;
            parent.SecondChild = child;

            child.FirstChild = parent;
            child.SecondChild = parent;

            var clonedParent = Cloneable<CloneableStub>.ShallowClone(parent);

            Assert.IsFalse(object.ReferenceEquals(parent, clonedParent));

            Assert.IsTrue(object.ReferenceEquals(parent.FirstChild, clonedParent.FirstChild));
            Assert.IsTrue(object.ReferenceEquals(parent.SecondChild, clonedParent.SecondChild));

            var clonedChild = clonedParent.FirstChild;

            Assert.IsFalse(object.ReferenceEquals(clonedChild.FirstChild, clonedParent));
            Assert.IsFalse(object.ReferenceEquals(clonedChild.SecondChild, clonedParent));

            Assert.IsTrue(object.ReferenceEquals(clonedChild.FirstChild, parent));
            Assert.IsTrue(object.ReferenceEquals(clonedChild.SecondChild, parent));
        }

        [Test]
        public void CloneReferenceOnly()
        {
            Guid id = Guid.NewGuid();
            DateTime birthDate = DateTime.Now.AddDays(-1);

            var stub = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };
            stub.ReferenceOnlyClone = stub;

            var clonedStub = Cloneable<CloneableStub>.Clone(stub);

            Assert.IsFalse(object.ReferenceEquals(stub, clonedStub));
            Assert.IsTrue(object.ReferenceEquals(stub, clonedStub.ReferenceOnlyClone));
        }
    }
}
