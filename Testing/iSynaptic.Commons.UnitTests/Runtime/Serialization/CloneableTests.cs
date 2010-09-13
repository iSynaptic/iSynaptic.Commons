using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using iSynaptic.Commons.Runtime.Serialization;
using iSynaptic.Commons.Testing.NUnit;

namespace iSynaptic.Commons.Runtime.Serialization
{
    [TestFixture]
    public class CloneableTests : NUnitBaseTestFixture
    {
        #region Nested Test Classes and Structures

        private class CloneTestClass
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public CloneTestClass InnerClass { get; set; }
        }

        private struct CloneTestStruct
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        private class ClassWithIntPtrField
        {
            public IntPtr CannotCloneThis { get; set; }
        }

        private class DerivedClassWithIntPtrField : ClassWithIntPtrField
        {
        }

        private class ClassWithDelegateField
        {
            public Action CannotCloneThis { get; set; }
        }

        private class DerivedClassWithDelegateField : ClassWithDelegateField
        {
        }

        private struct StructWithIntPtrField
        {
            public IntPtr CannotCloneThis { get; set; }
        }

        private class ReferenceOnlyCloneTestClass
        {
            [CloneReferenceOnly]
            public ReferenceOnlyCloneTestClass InnerClass = null;
        }

        private class ClassWithIntPtrArray
        {
            public IntPtr[] CannotCloneThis { get; set; }
        }

        private class ClassWithNullableIntPtr
        {
            public IntPtr? CannotCloneThis { get; set; }
        }

        private struct StructWithNullableIntPtr
        {
            public IntPtr? CannotCloneThis { get; set; }
        }

        private class ParentClass
        {
            public ChildClass Child { get; set; }
        }

        private class ChildClass
        {
            public string Name { get; set; }
        }

        private class ParentClassWithNonCloneableChild
        {
            public NonCloneableChild CannotCloneThis { get; set; }
        }

        private class NonCloneableChild
        {
            public IntPtr CannotCloneThis { get; set; }
        }

        private class ClassWithNonSerializedIllegalField
        {
            [NonSerialized]
            private IntPtr CannotCloneThis = IntPtr.Zero;

            public string Name { get; set; }
        }

        #endregion

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
            Assert.IsTrue(Cloneable<Int16?>.CanClone());
            Assert.IsTrue(Cloneable<Int16?>.CanShallowClone());

            Assert.AreEqual((Int16)(-16), Cloneable<Int16>.Clone(-16));
            Assert.AreEqual((Int16)(-16), Cloneable<Int16>.ShallowClone(-16));

            Assert.AreEqual((Int16?)(-16), Cloneable<Int16?>.Clone(-16));
            Assert.AreEqual((Int16?)(-16), Cloneable<Int16?>.ShallowClone(-16));
            Assert.AreEqual((Int16?)(null), Cloneable<Int16?>.Clone(null));
            Assert.AreEqual((Int16?)(null), Cloneable<Int16?>.ShallowClone(null));
        }

        [Test]
        public void CloneUInt16()
        {
            Assert.IsTrue(Cloneable<UInt16>.CanClone());
            Assert.IsTrue(Cloneable<UInt16>.CanShallowClone());
            Assert.IsTrue(Cloneable<UInt16?>.CanClone());
            Assert.IsTrue(Cloneable<UInt16?>.CanShallowClone());

            Assert.AreEqual((UInt16)16, Cloneable<UInt16>.Clone(16));
            Assert.AreEqual((UInt16)16, Cloneable<UInt16>.ShallowClone(16));

            Assert.AreEqual((UInt16?)16, Cloneable<UInt16?>.Clone(16));
            Assert.AreEqual((UInt16?)16, Cloneable<UInt16?>.ShallowClone(16));
            Assert.AreEqual((UInt16?)null, Cloneable<UInt16?>.Clone(null));
            Assert.AreEqual((UInt16?)null, Cloneable<UInt16?>.ShallowClone(null));
        }

        [Test]
        public void CloneInt32()
        {
            Assert.IsTrue(Cloneable<Int32>.CanClone());
            Assert.IsTrue(Cloneable<Int32>.CanShallowClone());
            Assert.IsTrue(Cloneable<Int32?>.CanClone());
            Assert.IsTrue(Cloneable<Int32?>.CanShallowClone());

            Assert.AreEqual((Int32)(-32), Cloneable<Int32>.Clone(-32));
            Assert.AreEqual((Int32)(-32), Cloneable<Int32>.ShallowClone(-32));

            Assert.AreEqual((Int32?)(-32), Cloneable<Int32?>.Clone(-32));
            Assert.AreEqual((Int32?)(-32), Cloneable<Int32?>.ShallowClone(-32));
            Assert.AreEqual((Int32?)(null), Cloneable<Int32?>.Clone(null));
            Assert.AreEqual((Int32?)(null), Cloneable<Int32?>.ShallowClone(null));
        }

        [Test]
        public void CloneUInt32()
        {
            Assert.IsTrue(Cloneable<UInt32>.CanClone());
            Assert.IsTrue(Cloneable<UInt32>.CanShallowClone());
            Assert.IsTrue(Cloneable<UInt32?>.CanClone());
            Assert.IsTrue(Cloneable<UInt32?>.CanShallowClone());

            Assert.AreEqual((UInt32)32, Cloneable<UInt32>.Clone(32));
            Assert.AreEqual((UInt32)32, Cloneable<UInt32>.ShallowClone(32));

            Assert.AreEqual((UInt32?)32, Cloneable<UInt32?>.Clone(32));
            Assert.AreEqual((UInt32?)32, Cloneable<UInt32?>.ShallowClone(32));
            Assert.AreEqual((UInt32?)null, Cloneable<UInt32?>.Clone(null));
            Assert.AreEqual((UInt32?)null, Cloneable<UInt32?>.ShallowClone(null));
        }

        [Test]
        public void CloneInt64()
        {
            Assert.IsTrue(Cloneable<Int64>.CanClone());
            Assert.IsTrue(Cloneable<Int64>.CanShallowClone());
            Assert.IsTrue(Cloneable<Int64?>.CanClone());
            Assert.IsTrue(Cloneable<Int64?>.CanShallowClone());

            Assert.AreEqual((Int64)(-64), Cloneable<Int64>.Clone(-64));
            Assert.AreEqual((Int64)(-64), Cloneable<Int64>.ShallowClone(-64));

            Assert.AreEqual((Int64?)(-64), Cloneable<Int64?>.Clone(-64));
            Assert.AreEqual((Int64?)(-64), Cloneable<Int64?>.ShallowClone(-64));
            Assert.AreEqual((Int64?)(null), Cloneable<Int64?>.Clone(null));
            Assert.AreEqual((Int64?)(null), Cloneable<Int64?>.ShallowClone(null));
        }

        [Test]
        public void CloneUInt64()
        {
            Assert.IsTrue(Cloneable<UInt64>.CanClone());
            Assert.IsTrue(Cloneable<UInt64>.CanShallowClone());
            Assert.IsTrue(Cloneable<UInt64?>.CanClone());
            Assert.IsTrue(Cloneable<UInt64?>.CanShallowClone());

            Assert.AreEqual((UInt64)64, Cloneable<UInt64>.Clone(64));
            Assert.AreEqual((UInt64)64, Cloneable<UInt64>.ShallowClone(64));

            Assert.AreEqual((UInt64?)64, Cloneable<UInt64?>.Clone(64));
            Assert.AreEqual((UInt64?)64, Cloneable<UInt64?>.ShallowClone(64));
            Assert.AreEqual((UInt64?)null, Cloneable<UInt64?>.Clone(null));
            Assert.AreEqual((UInt64?)null, Cloneable<UInt64?>.ShallowClone(null));
        }

        [Test]
        public void CloneByte()
        {
            Assert.IsTrue(Cloneable<Byte>.CanClone());
            Assert.IsTrue(Cloneable<Byte>.CanShallowClone());
            Assert.IsTrue(Cloneable<Byte?>.CanClone());
            Assert.IsTrue(Cloneable<Byte?>.CanShallowClone());

            Assert.AreEqual((Byte)(8), Cloneable<Byte>.Clone(8));
            Assert.AreEqual((Byte)(8), Cloneable<Byte>.ShallowClone(8));

            Assert.AreEqual((Byte?)(8), Cloneable<Byte?>.Clone(8));
            Assert.AreEqual((Byte?)(8), Cloneable<Byte?>.ShallowClone(8));
            Assert.AreEqual((Byte?)(null), Cloneable<Byte?>.Clone(null));
            Assert.AreEqual((Byte?)(null), Cloneable<Byte?>.ShallowClone(null));
        }

        [Test]
        public void CloneSByte()
        {
            Assert.IsTrue(Cloneable<SByte>.CanClone());
            Assert.IsTrue(Cloneable<SByte>.CanShallowClone());
            Assert.IsTrue(Cloneable<SByte?>.CanClone());
            Assert.IsTrue(Cloneable<SByte?>.CanShallowClone());

            Assert.AreEqual((SByte)(-8), Cloneable<SByte>.Clone(-8));
            Assert.AreEqual((SByte)(-8), Cloneable<SByte>.ShallowClone(-8));

            Assert.AreEqual((SByte?)(-8), Cloneable<SByte?>.Clone(-8));
            Assert.AreEqual((SByte?)(-8), Cloneable<SByte?>.ShallowClone(-8));
            Assert.AreEqual((SByte?)(null), Cloneable<SByte?>.Clone(null));
            Assert.AreEqual((SByte?)(null), Cloneable<SByte?>.ShallowClone(null));
        }

        [Test]
        public void CloneBoolean()
        {
            Assert.IsTrue(Cloneable<Boolean>.CanClone());
            Assert.IsTrue(Cloneable<Boolean>.CanShallowClone());
            Assert.IsTrue(Cloneable<Boolean?>.CanClone());
            Assert.IsTrue(Cloneable<Boolean?>.CanShallowClone());

            Assert.AreEqual(true, Cloneable<Boolean>.Clone(true));
            Assert.AreEqual(true, Cloneable<Boolean>.ShallowClone(true));

            Assert.AreEqual(true, Cloneable<Boolean?>.Clone(true));
            Assert.AreEqual(true, Cloneable<Boolean?>.ShallowClone(true));
            Assert.AreEqual(null, Cloneable<Boolean?>.Clone(null));
            Assert.AreEqual(null, Cloneable<Boolean?>.ShallowClone(null));
        }

        [Test]
        public void CloneDouble()
        {
            Assert.IsTrue(Cloneable<Double>.CanClone());
            Assert.IsTrue(Cloneable<Double>.CanShallowClone());

            Assert.IsTrue(Cloneable<Double?>.CanClone());
            Assert.IsTrue(Cloneable<Double?>.CanShallowClone());

            Assert.AreEqual((Double)128, Cloneable<Double>.Clone(128));
            Assert.AreEqual((Double)128, Cloneable<Double>.ShallowClone(128));

            Assert.AreEqual((Double?)128, Cloneable<Double?>.Clone(128));
            Assert.AreEqual((Double?)128, Cloneable<Double?>.ShallowClone(128));
            Assert.AreEqual((Double?)null, Cloneable<Double?>.Clone(null));
            Assert.AreEqual((Double?)null, Cloneable<Double?>.ShallowClone(null));
        }

        [Test]
        public void CloneDecimal()
        {
            Assert.IsTrue(Cloneable<decimal>.CanClone());
            Assert.IsTrue(Cloneable<decimal>.CanShallowClone());

            Assert.IsTrue(Cloneable<decimal?>.CanClone());
            Assert.IsTrue(Cloneable<decimal?>.CanShallowClone());


            Assert.AreEqual((decimal)128, Cloneable<decimal>.Clone(128));
            Assert.AreEqual((decimal)128, Cloneable<decimal>.ShallowClone(128));

            Assert.AreEqual((decimal?)128, Cloneable<decimal?>.Clone(128));
            Assert.AreEqual((decimal?)128, Cloneable<decimal?>.ShallowClone(128));
            Assert.AreEqual((decimal?)null, Cloneable<decimal?>.Clone(null));
            Assert.AreEqual((decimal?)null, Cloneable<decimal?>.ShallowClone(null));
        }

        [Test]
        public void CloneChar()
        {
            Assert.IsTrue(Cloneable<Char>.CanClone());
            Assert.IsTrue(Cloneable<Char>.CanShallowClone());

            Assert.IsTrue(Cloneable<Char?>.CanClone());
            Assert.IsTrue(Cloneable<Char?>.CanShallowClone());

            Assert.AreEqual('A', Cloneable<Char>.Clone('A'));
            Assert.AreEqual('A', Cloneable<Char>.ShallowClone('A'));

            Assert.AreEqual('A', Cloneable<Char?>.Clone('A'));
            Assert.AreEqual('A', Cloneable<Char?>.ShallowClone('A'));
            Assert.AreEqual(null, Cloneable<Char?>.Clone(null));
            Assert.AreEqual(null, Cloneable<Char?>.ShallowClone(null));
        }

        [Test]
        public void CloneSingle()
        {
            Assert.IsTrue(Cloneable<Single>.CanClone());
            Assert.IsTrue(Cloneable<Single>.CanShallowClone());

            Assert.IsTrue(Cloneable<Single?>.CanClone());
            Assert.IsTrue(Cloneable<Single?>.CanShallowClone());

            Assert.AreEqual((Single)64, Cloneable<Single>.Clone(64));
            Assert.AreEqual((Single)64, Cloneable<Single>.ShallowClone(64));

            Assert.AreEqual((Single?)64, Cloneable<Single?>.Clone(64));
            Assert.AreEqual((Single?)64, Cloneable<Single?>.ShallowClone(64));
            Assert.AreEqual((Single?)null, Cloneable<Single?>.Clone(null));
            Assert.AreEqual((Single?)null, Cloneable<Single?>.ShallowClone(null));
        }

        [Test]
        public void CloneGuid()
        {
            Guid id = new Guid("6C31A07E-F633-479b-836B-9479CD7EF92F");

            Assert.IsTrue(Cloneable<Guid>.CanClone());
            Assert.IsTrue(Cloneable<Guid>.CanShallowClone());

            Assert.IsTrue(Cloneable<Guid?>.CanClone());
            Assert.IsTrue(Cloneable<Guid?>.CanShallowClone());

            Assert.AreEqual(id, Cloneable<Guid>.Clone(id));
            Assert.AreEqual(id, Cloneable<Guid>.ShallowClone(id));

            Assert.AreEqual(id, Cloneable<Guid?>.Clone(id));
            Assert.AreEqual(id, Cloneable<Guid?>.ShallowClone(id));
            Assert.AreEqual(null, Cloneable<Guid?>.Clone(null));
            Assert.AreEqual(null, Cloneable<Guid?>.ShallowClone(null));
        }

        [Test]
        public void CloneDateTime()
        {
            DateTime now = DateTime.Now;

            Assert.IsTrue(Cloneable<DateTime>.CanClone());
            Assert.IsTrue(Cloneable<DateTime>.CanShallowClone());

            Assert.IsTrue(Cloneable<DateTime?>.CanClone());
            Assert.IsTrue(Cloneable<DateTime?>.CanShallowClone());

            Assert.AreEqual(now, Cloneable<DateTime>.Clone(now));
            Assert.AreEqual(now, Cloneable<DateTime>.ShallowClone(now));

            Assert.AreEqual(now, Cloneable<DateTime?>.Clone(now));
            Assert.AreEqual(now, Cloneable<DateTime?>.ShallowClone(now));
            Assert.AreEqual(null, Cloneable<DateTime?>.Clone(null));
            Assert.AreEqual(null, Cloneable<DateTime?>.ShallowClone(null));
        }

        [Test]
        public void CloneTimeSpan()
        {
            TimeSpan source = TimeSpan.FromMinutes(5);

            Assert.IsTrue(Cloneable<TimeSpan>.CanClone());
            Assert.IsTrue(Cloneable<TimeSpan>.CanShallowClone());

            Assert.IsTrue(Cloneable<TimeSpan?>.CanClone());
            Assert.IsTrue(Cloneable<TimeSpan?>.CanShallowClone());

            Assert.AreEqual(source, Cloneable<TimeSpan>.Clone(source));
            Assert.AreEqual(source, Cloneable<TimeSpan>.ShallowClone(source));

            Assert.AreEqual(source, Cloneable<TimeSpan?>.Clone(source));
            Assert.AreEqual(source, Cloneable<TimeSpan?>.ShallowClone(source));
            Assert.AreEqual(null, Cloneable<TimeSpan?>.Clone(null));
            Assert.AreEqual(null, Cloneable<TimeSpan?>.ShallowClone(null));
        }

        [Test]
        public void CloneListOfValueType()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5 };

            Assert.IsTrue(Cloneable<List<int>>.CanClone());
            Assert.IsTrue(Cloneable<List<int>>.CanShallowClone());

            List<int> clone = Cloneable<List<int>>.Clone(source);
            List<int> shallowClone = Cloneable<List<int>>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.IsTrue(clone.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
            Assert.IsTrue(shallowClone.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void CloneListOfReferenceType()
        {
            List<CloneTestClass> source = new List<CloneTestClass> { new CloneTestClass { FirstName = "John", LastName = "Doe" } };

            Assert.IsTrue(Cloneable<List<CloneTestClass>>.CanClone());
            Assert.IsTrue(Cloneable<List<CloneTestClass>>.CanShallowClone());

            List<CloneTestClass> clone = Cloneable<List<CloneTestClass>>.Clone(source);
            List<CloneTestClass> shallowClone = Cloneable<List<CloneTestClass>>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.AreEqual(1, clone.Count);
            Assert.AreEqual(1, shallowClone.Count);

            Assert.IsFalse(object.ReferenceEquals(source[0], clone[0]));
            Assert.IsTrue(object.ReferenceEquals(source[0], shallowClone[0]));

            Assert.AreEqual("John", clone[0].FirstName);
            Assert.AreEqual("Doe", clone[0].LastName);
        }

        [Test]
        public void CannotCloneIntPtr()
        {
            Assert.IsFalse(Cloneable<IntPtr>.CanClone());
            Assert.IsFalse(Cloneable<IntPtr>.CanShallowClone());

            Assert.IsFalse(Cloneable<IntPtr?>.CanClone());
            Assert.IsFalse(Cloneable<IntPtr?>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<IntPtr>.Clone(IntPtr.Zero));
            Assert.Throws<InvalidOperationException>(() => Cloneable<IntPtr>.ShallowClone(IntPtr.Zero));

            Assert.Throws<InvalidOperationException>(() => Cloneable<IntPtr?>.Clone(IntPtr.Zero));
            Assert.Throws<InvalidOperationException>(() => Cloneable<IntPtr?>.ShallowClone(IntPtr.Zero));
            Assert.Throws<InvalidOperationException>(() => Cloneable<IntPtr?>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<IntPtr?>.ShallowClone(null));
        }

        [Test]
        public void CannotCloneDelegate()
        {
            Assert.IsFalse(Cloneable<Action>.CanClone());
            Assert.IsFalse(Cloneable<Action>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<Action>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<Action>.ShallowClone(null));
        }

        [Test]
        public void ClonePrimitiveArray()
        {
            int[] ints = new int[] { 1, 2, 3, 4, 5 };

            Assert.IsTrue(Cloneable<int[]>.CanClone());
            Assert.IsTrue(Cloneable<int[]>.CanShallowClone());

            int[] clonedInts = Cloneable<int[]>.Clone(ints);
            int[] shallowClonedInts = Cloneable<int[]>.ShallowClone(ints);

            Assert.IsFalse(object.ReferenceEquals(ints, clonedInts));
            Assert.IsFalse(object.ReferenceEquals(ints, shallowClonedInts));

            Assert.IsTrue(clonedInts.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
            Assert.IsTrue(shallowClonedInts.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void CloneClass()
        {
            var source = new CloneTestClass { FirstName = "John", LastName = "Doe" };

            Assert.IsTrue(Cloneable<CloneTestClass>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestClass>.CanShallowClone());

            var clone = Cloneable<CloneTestClass>.Clone(source);
            var shallowClone = Cloneable<CloneTestClass>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.AreEqual("John", clone.FirstName);
            Assert.AreEqual("Doe", clone.LastName);

            Assert.AreEqual("John", shallowClone.FirstName);
            Assert.AreEqual("Doe", shallowClone.LastName);
        }

        [Test]
        public void CloneNullClass()
        {
            var clone = Cloneable<CloneTestClass>.Clone(null);
            var shallowClone = Cloneable<CloneTestClass>.ShallowClone(null);

            Assert.IsNull(clone);
            Assert.IsNull(shallowClone);
        }

        [Test]
        public void CloneSelfReferencingClass()
        {
            var source = new CloneTestClass { FirstName = "John", LastName = "Doe" };
            source.InnerClass = source;

            var clone = Cloneable<CloneTestClass>.Clone(source);
            var shallowClone = Cloneable<CloneTestClass>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.IsTrue(object.ReferenceEquals(clone, clone.InnerClass));
            Assert.IsTrue(object.ReferenceEquals(shallowClone, shallowClone.InnerClass));
        }

        [Test]
        public void CloneStruct()
        {
            var source = new CloneTestStruct { FirstName = "John", LastName = "Doe" };

            Assert.IsTrue(Cloneable<CloneTestStruct>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestStruct>.CanShallowClone());

            Assert.IsTrue(Cloneable<CloneTestStruct?>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestStruct?>.CanShallowClone());

            var clone = Cloneable<CloneTestStruct>.Clone(source);
            var shallowClone = Cloneable<CloneTestStruct>.ShallowClone(source);

            Assert.AreEqual("John", clone.FirstName);
            Assert.AreEqual("Doe", clone.LastName);

            Assert.AreEqual("John", shallowClone.FirstName);
            Assert.AreEqual("Doe", shallowClone.LastName);

            clone = Cloneable<CloneTestStruct?>.Clone(source).Value;
            shallowClone = Cloneable<CloneTestStruct?>.ShallowClone(source).Value;

            Assert.AreEqual("John", clone.FirstName);
            Assert.AreEqual("Doe", clone.LastName);

            Assert.AreEqual("John", shallowClone.FirstName);
            Assert.AreEqual("Doe", shallowClone.LastName);

            Assert.IsNull(Cloneable<CloneTestStruct?>.Clone(null));
            Assert.IsNull(Cloneable<CloneTestStruct?>.ShallowClone(null));
        }

        [Test]
        public void CloneClassArray()
        {
            var source = new CloneTestClass[]
            {
                new CloneTestClass { FirstName = "John", LastName = "Doe" },
                new CloneTestClass { FirstName = "Jane", LastName = "Smith"}
            };

            Assert.IsTrue(Cloneable<CloneTestClass[]>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestClass[]>.CanShallowClone());

            var clones = Cloneable<CloneTestClass[]>.Clone(source);
            var shallowClones = Cloneable<CloneTestClass[]>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clones));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClones));

            Assert.AreEqual(source.Length, clones.Length);
            Assert.AreEqual(source.Length, shallowClones.Length);

            Assert.IsFalse(object.ReferenceEquals(source[0], clones[0]));
            Assert.IsFalse(object.ReferenceEquals(source[1], clones[1]));

            Assert.IsTrue(object.ReferenceEquals(source[0], shallowClones[0]));
            Assert.IsTrue(object.ReferenceEquals(source[1], shallowClones[1]));

            Assert.AreEqual("John", clones[0].FirstName);
            Assert.AreEqual("Doe", clones[0].LastName);

            Assert.AreEqual("Jane", clones[1].FirstName);
            Assert.AreEqual("Smith", clones[1].LastName);
        }

        [Test]
        public void CloneStructArray()
        {
            var source = new CloneTestStruct[]
            {
                new CloneTestStruct { FirstName = "John", LastName = "Doe" },
                new CloneTestStruct { FirstName = "Jane", LastName = "Smith"}
            };

            Assert.IsTrue(Cloneable<CloneTestStruct[]>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestStruct[]>.CanShallowClone());

            var clones = Cloneable<CloneTestStruct[]>.Clone(source);
            var shallowClones = Cloneable<CloneTestStruct[]>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clones));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClones));

            Assert.AreEqual(source.Length, clones.Length);
            Assert.AreEqual(source.Length, shallowClones.Length);

            Assert.AreEqual("John", clones[0].FirstName);
            Assert.AreEqual("Doe", clones[0].LastName);

            Assert.AreEqual("Jane", clones[1].FirstName);
            Assert.AreEqual("Smith", clones[1].LastName);
        }


        [Test]
        public void CloneMultidimensionalPrimitiveArray()
        {
            int[,] source = new int[,] { { 1, 2, 3 } };

            Assert.IsTrue(Cloneable<int[,]>.CanClone());
            Assert.IsTrue(Cloneable<int[,]>.CanShallowClone());

            var clone = Cloneable<int[,]>.Clone(source);
            var shallowClone = Cloneable<int[,]>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.AreEqual(source.Length, clone.Length);
            Assert.AreEqual(source.Length, shallowClone.Length);

            Assert.IsTrue(source[0, 0].Equals(clone[0, 0]));
            Assert.IsTrue(source[0, 1].Equals(clone[0, 1]));
            Assert.IsTrue(source[0, 2].Equals(clone[0, 2]));

            Assert.IsTrue(source[0, 0].Equals(shallowClone[0, 0]));
            Assert.IsTrue(source[0, 1].Equals(shallowClone[0, 1]));
            Assert.IsTrue(source[0, 2].Equals(shallowClone[0, 2]));
        }

        [Test]
        public void CloneMultidimensionalClassArray()
        {
            CloneTestClass[,] source = new CloneTestClass[,]
            {
                {
                    new CloneTestClass { FirstName = "John", LastName = "Doe" },
                    new CloneTestClass { FirstName = "Jane", LastName = "Smith"}
                }
            };

            source[0, 1].InnerClass = source[0, 1];

            Assert.IsTrue(Cloneable<CloneTestClass[,]>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestClass[,]>.CanShallowClone());

            var clone = Cloneable<CloneTestClass[,]>.Clone(source);
            var shallowClone = Cloneable<CloneTestClass[,]>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.AreEqual(source.Length, clone.Length);
            Assert.AreEqual(source.Length, shallowClone.Length);

            Assert.IsFalse(object.ReferenceEquals(source[0, 0], clone[0, 0]));
            Assert.IsFalse(object.ReferenceEquals(source[0, 1], clone[0, 1]));
            Assert.IsFalse(object.ReferenceEquals(source[0, 1], clone[0, 1].InnerClass));
            Assert.IsTrue(object.ReferenceEquals(clone[0, 1], clone[0, 1].InnerClass));

            Assert.IsTrue(object.ReferenceEquals(source[0, 0], shallowClone[0, 0]));
            Assert.IsTrue(object.ReferenceEquals(source[0, 1], shallowClone[0, 1]));
            Assert.IsTrue(object.ReferenceEquals(source[0, 1], shallowClone[0, 1].InnerClass));
            Assert.IsFalse(object.ReferenceEquals(clone[0, 1], shallowClone[0, 1].InnerClass));

            Assert.AreEqual("John", clone[0, 0].FirstName);
            Assert.AreEqual("Doe", clone[0, 0].LastName);

            Assert.AreEqual("Jane", clone[0, 1].FirstName);
            Assert.AreEqual("Smith", clone[0, 1].LastName);

            Assert.AreEqual("John", shallowClone[0, 0].FirstName);
            Assert.AreEqual("Doe", shallowClone[0, 0].LastName);

            Assert.AreEqual("Jane", shallowClone[0, 1].FirstName);
            Assert.AreEqual("Smith", shallowClone[0, 1].LastName);
        }

        [Test]
        public void CloneMultidimensionalStructArray()
        {
            CloneTestStruct[,] source = new CloneTestStruct[,]
            {
                {
                    new CloneTestStruct { FirstName = "John", LastName = "Doe" },
                    new CloneTestStruct { FirstName = "Jane", LastName = "Smith"}
                }
            };

            Assert.IsTrue(Cloneable<CloneTestStruct[,]>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestStruct[,]>.CanShallowClone());

            var clone = Cloneable<CloneTestStruct[,]>.Clone(source);
            var shallowClone = Cloneable<CloneTestStruct[,]>.ShallowClone(source);

            Assert.AreEqual(source.Length, clone.Length);
            Assert.AreEqual(source.Length, shallowClone.Length);

            Assert.AreEqual("John", clone[0, 0].FirstName);
            Assert.AreEqual("Doe", clone[0, 0].LastName);

            Assert.AreEqual("Jane", clone[0, 1].FirstName);
            Assert.AreEqual("Smith", clone[0, 1].LastName);

            Assert.AreEqual("John", shallowClone[0, 0].FirstName);
            Assert.AreEqual("Doe", shallowClone[0, 0].LastName);

            Assert.AreEqual("Jane", shallowClone[0, 1].FirstName);
            Assert.AreEqual("Smith", shallowClone[0, 1].LastName);
        }

        [Test]
        public void CloneJaggedPrimitiveArray()
        {
            var source = new int[][]
            {
                new int[]{1,2,3,4,5}
            };

            Assert.IsTrue(Cloneable<int[][]>.CanClone());
            Assert.IsTrue(Cloneable<int[][]>.CanShallowClone());

            var clone = Cloneable<int[][]>.Clone(source);
            var shallowClone = Cloneable<int[][]>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.AreEqual(source.Length, clone.Length);
            Assert.AreEqual(source.Length, shallowClone.Length);

            Assert.AreEqual(source[0][0], clone[0][0]);
            Assert.AreEqual(source[0][1], clone[0][1]);
            Assert.AreEqual(source[0][2], clone[0][2]);

            Assert.AreEqual(source[0][0], shallowClone[0][0]);
            Assert.AreEqual(source[0][1], shallowClone[0][1]);
            Assert.AreEqual(source[0][2], shallowClone[0][2]);
        }

        [Test]
        public void CloneJaggedClassArray()
        {
            var source = new CloneTestClass[][]
            {
                new CloneTestClass[]
                {
                    new CloneTestClass { FirstName = "John", LastName = "Doe" },
                    new CloneTestClass { FirstName = "Jane", LastName = "Smith"}
                }

            };

            Assert.IsTrue(Cloneable<CloneTestClass[][]>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestClass[][]>.CanShallowClone());

            var clone = Cloneable<CloneTestClass[][]>.Clone(source);
            var shallowClone = Cloneable<CloneTestClass[][]>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.IsFalse(object.ReferenceEquals(source[0], clone[0]));
            Assert.IsFalse(object.ReferenceEquals(source[0][0], clone[0][0]));
            Assert.IsFalse(object.ReferenceEquals(source[0][1], clone[0][1]));

            Assert.IsTrue(object.ReferenceEquals(source[0], shallowClone[0]));
            Assert.IsTrue(object.ReferenceEquals(source[0][0], shallowClone[0][0]));
            Assert.IsTrue(object.ReferenceEquals(source[0][1], shallowClone[0][1]));

            Assert.AreEqual(source.Length, clone.Length);
            Assert.AreEqual(source.Length, shallowClone.Length);

            Assert.AreEqual("John", clone[0][0].FirstName);
            Assert.AreEqual("Doe", clone[0][0].LastName);
            Assert.AreEqual("Jane", clone[0][1].FirstName);
            Assert.AreEqual("Smith", clone[0][1].LastName);
        }

        [Test]
        public void CloneJaggedStructArray()
        {
            var source = new CloneTestStruct[][]
            {
                new CloneTestStruct[]
                {
                    new CloneTestStruct { FirstName = "John", LastName = "Doe" },
                    new CloneTestStruct { FirstName = "Jane", LastName = "Smith"}
                }

            };

            Assert.IsTrue(Cloneable<CloneTestStruct[][]>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestStruct[][]>.CanShallowClone());

            var clone = Cloneable<CloneTestStruct[][]>.Clone(source);
            var shallowClone = Cloneable<CloneTestStruct[][]>.ShallowClone(source);

            Assert.AreEqual(source.Length, clone.Length);
            Assert.AreEqual(source.Length, shallowClone.Length);

            Assert.AreEqual("John", clone[0][0].FirstName);
            Assert.AreEqual("Doe", clone[0][0].LastName);
            Assert.AreEqual("Jane", clone[0][1].FirstName);
            Assert.AreEqual("Smith", clone[0][1].LastName);

            Assert.AreEqual("John", shallowClone[0][0].FirstName);
            Assert.AreEqual("Doe", shallowClone[0][0].LastName);
            Assert.AreEqual("Jane", shallowClone[0][1].FirstName);
            Assert.AreEqual("Smith", shallowClone[0][1].LastName);
        }

        [Test]
        public void CannotCloneClassWithIntPtrField()
        {
            Assert.IsFalse(Cloneable<ClassWithIntPtrField>.CanClone());
            Assert.IsFalse(Cloneable<ClassWithIntPtrField>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithIntPtrField>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithIntPtrField>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithIntPtrField>.Clone(new ClassWithIntPtrField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithIntPtrField>.ShallowClone(new ClassWithIntPtrField()));
        }

        [Test]
        public void CannotCloneDerivedClassWithIntPtrField()
        {
            Assert.IsFalse(Cloneable<DerivedClassWithIntPtrField>.CanClone());
            Assert.IsFalse(Cloneable<DerivedClassWithIntPtrField>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithIntPtrField>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithIntPtrField>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithIntPtrField>.Clone(new DerivedClassWithIntPtrField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithIntPtrField>.ShallowClone(new DerivedClassWithIntPtrField()));
        }

        [Test]
        public void CannotCloneClassWithDelegateField()
        {
            Assert.IsFalse(Cloneable<ClassWithDelegateField>.CanClone());
            Assert.IsFalse(Cloneable<ClassWithDelegateField>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithDelegateField>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithDelegateField>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithDelegateField>.Clone(new ClassWithDelegateField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithDelegateField>.ShallowClone(new ClassWithDelegateField()));
        }

        [Test]
        public void CannotCloneDerivedClassWithDeletegateField()
        {
            Assert.IsFalse(Cloneable<DerivedClassWithDelegateField>.CanClone());
            Assert.IsFalse(Cloneable<DerivedClassWithDelegateField>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithDelegateField>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithDelegateField>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithDelegateField>.Clone(new DerivedClassWithDelegateField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithDelegateField>.ShallowClone(new DerivedClassWithDelegateField()));
        }

        [Test]
        public void CannotCloneStructWithIntPtrField()
        {
            Assert.IsFalse(Cloneable<StructWithIntPtrField>.CanClone());
            Assert.IsFalse(Cloneable<StructWithIntPtrField>.CanShallowClone());

            Assert.IsFalse(Cloneable<StructWithIntPtrField?>.CanClone());
            Assert.IsFalse(Cloneable<StructWithIntPtrField?>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithIntPtrField>.Clone(new StructWithIntPtrField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithIntPtrField>.ShallowClone(new StructWithIntPtrField()));
            
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithIntPtrField?>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithIntPtrField?>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithIntPtrField?>.Clone(new StructWithIntPtrField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithIntPtrField?>.ShallowClone(new StructWithIntPtrField()));
        }

        [Test]
        public void CloneBidirectionalReferences()
        {
            var left = new CloneTestClass();
            var right = new CloneTestClass();

            left.InnerClass = right;
            right.InnerClass = left;

            var leftClone = Cloneable<CloneTestClass>.Clone(left);
            var rightClone = leftClone.InnerClass;

            Assert.IsFalse(object.ReferenceEquals(right, rightClone));
            Assert.IsFalse(object.ReferenceEquals(rightClone.InnerClass, left));
            Assert.IsTrue(object.ReferenceEquals(rightClone.InnerClass, leftClone));

            leftClone = Cloneable<CloneTestClass>.ShallowClone(left);
            rightClone = leftClone.InnerClass;

            Assert.IsTrue(object.ReferenceEquals(right, rightClone));
            Assert.IsTrue(object.ReferenceEquals(rightClone.InnerClass, left));
            Assert.IsFalse(object.ReferenceEquals(rightClone.InnerClass, leftClone));
        }


        [Test]
        public void CloneMultipleReferencesToSameArray()
        {
            var child = new CloneTestClass[]
            {
                new CloneTestClass(),
                new CloneTestClass()
            };

            var source = new CloneTestClass[][]
            {
                child,
                child
            };

            var clone = Cloneable<CloneTestClass[][]>.Clone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(clone[0], child));
            Assert.IsFalse(object.ReferenceEquals(clone[1], child));

            Assert.IsTrue(object.ReferenceEquals(clone[0], clone[1]));
        }

        [Test]
        public void CloneReferenceOnly()
        {
            var source = new ReferenceOnlyCloneTestClass();
            source.InnerClass = new ReferenceOnlyCloneTestClass();

            var clone = Cloneable<ReferenceOnlyCloneTestClass>.Clone(source);
            
            Assert.IsTrue(object.ReferenceEquals(source.InnerClass, clone.InnerClass));
        }

        [Test]
        public void CannotCloneClassWithIllegalArrayFieldType()
        {
            Assert.IsFalse(Cloneable<ClassWithIntPtrArray>.CanClone());
            Assert.IsFalse(Cloneable<ClassWithIntPtrArray>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithIntPtrArray>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithIntPtrArray>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithIntPtrArray>.Clone(new ClassWithIntPtrArray()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithIntPtrArray>.ShallowClone(new ClassWithIntPtrArray()));
        }

        [Test]
        public void CannotCloneClassWithIllegalNullableFieldType()
        {
            Assert.IsFalse(Cloneable<ClassWithNullableIntPtr>.CanClone());
            Assert.IsFalse(Cloneable<ClassWithNullableIntPtr>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithNullableIntPtr>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithNullableIntPtr>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithNullableIntPtr>.Clone(new ClassWithNullableIntPtr()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithNullableIntPtr>.ShallowClone(new ClassWithNullableIntPtr()));
        }

        [Test]
        public void CannotCloneStructWithIllegalNullableFieldType()
        {
            Assert.IsFalse(Cloneable<StructWithNullableIntPtr>.CanClone());
            Assert.IsFalse(Cloneable<StructWithNullableIntPtr>.CanShallowClone());

            Assert.IsFalse(Cloneable<StructWithNullableIntPtr?>.CanClone());
            Assert.IsFalse(Cloneable<StructWithNullableIntPtr?>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithNullableIntPtr>.Clone(new StructWithNullableIntPtr()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithNullableIntPtr>.ShallowClone(new StructWithNullableIntPtr()));

            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithNullableIntPtr?>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithNullableIntPtr?>.ShallowClone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithNullableIntPtr?>.Clone(new StructWithNullableIntPtr()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithNullableIntPtr?>.ShallowClone(new StructWithNullableIntPtr()));

        }

        [Test]
        public void CloneNullArray()
        {
            Assert.IsNull(Cloneable<int[]>.Clone(null));
            Assert.IsNull(Cloneable<int[]>.ShallowClone(null));
        }

        [Test]
        public void CloneEmptyArray()
        {
            var source = new int[] { };
            var clone = Cloneable<int[]>.Clone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.AreEqual(0, clone.Length);

            clone = Cloneable<int[]>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.AreEqual(0, clone.Length);
        }

        [Test]
        public void CloneObjectHierarchy()
        {
            var parent = new ParentClass();
            parent.Child = new ChildClass { Name = "John Doe" };

            Assert.IsTrue(Cloneable<ParentClass>.CanClone());
            Assert.IsTrue(Cloneable<ParentClass>.CanShallowClone());

            var parentClone = Cloneable<ParentClass>.Clone(parent);
            
            Assert.IsFalse(object.ReferenceEquals(parent.Child, parentClone.Child));
            Assert.AreEqual("John Doe", parentClone.Child.Name);
        }


        [Test]
        public void CannotCloneClassHierarchyWithIllegalFieldType()
        {
            Assert.IsFalse(Cloneable<ParentClassWithNonCloneableChild>.CanClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<ParentClassWithNonCloneableChild>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ParentClassWithNonCloneableChild>.Clone(new ParentClassWithNonCloneableChild()));
        }

        [Test]
        public void ShallowCloneClassHierarchyWithIllegalFieldTypeInReferencedClass()
        {
            Assert.IsTrue(Cloneable<ParentClassWithNonCloneableChild>.CanShallowClone());

            Assert.IsNull(Cloneable<ParentClassWithNonCloneableChild>.ShallowClone(null));
            Assert.IsNotNull(Cloneable<ParentClassWithNonCloneableChild>.ShallowClone(new ParentClassWithNonCloneableChild()));
        }

        [Test]
        public void CloneClassWithNonSerializedIllegalField()
        {
            Assert.IsTrue(Cloneable<ClassWithNonSerializedIllegalField>.CanClone());
            Assert.IsTrue(Cloneable<ClassWithNonSerializedIllegalField>.CanShallowClone());

            var source = new ClassWithNonSerializedIllegalField { Name = "John Doe" };

            var clone = Cloneable<ClassWithNonSerializedIllegalField>.Clone(source);
            Assert.IsNotNull(clone);

            clone = null;

            clone = Cloneable<ClassWithNonSerializedIllegalField>.ShallowClone(source);
            Assert.IsNotNull(clone);
        }

        [Test]
        public void CloneToWillNotWorkOnAValueType()
        {
            Assert.Throws<InvalidOperationException>(() => Cloneable<int>.CloneTo(1, 1));
            Assert.Throws<InvalidOperationException>(() => Cloneable<CloneTestStruct>.CloneTo(new CloneTestStruct(), new CloneTestStruct()));
        }

        [Test]
        public void ShallowCloneToWillNotWorkOnAValueType()
        {
            Assert.Throws<InvalidOperationException>(() => Cloneable<int>.ShallowCloneTo(1, 1));
            Assert.Throws<InvalidOperationException>(() => Cloneable<CloneTestStruct>.ShallowCloneTo(new CloneTestStruct(), new CloneTestStruct()));
        }

        [Test]
        public void CloneToWillNotWorkWithNullReferences()
        {
            Assert.Throws<ArgumentNullException>(() => Cloneable<ParentClass>.CloneTo(new ParentClass(), null));
            Assert.Throws<ArgumentNullException>(() => Cloneable<ParentClass>.CloneTo(null, new ParentClass()));
        }

        [Test]
        public void ShallowCloneToWillNotWorkWithNullReferences()
        {
            Assert.Throws<ArgumentNullException>(() => Cloneable<ParentClass>.ShallowCloneTo(new ParentClass(), null));
            Assert.Throws<ArgumentNullException>(() => Cloneable<ParentClass>.ShallowCloneTo(null, new ParentClass()));
        }

        [Test]
        public void CloneToWithSameDestinationAsSourceWillNotWork()
        {
            var testClass = new ParentClass();
            Assert.Throws<InvalidOperationException>(() => Cloneable<ParentClass>.CloneTo(testClass, testClass));
        }

        [Test]
        public void ShallowCloneToWithSameDestinationAsSourceWillNotWork()
        {
            var testClass = new ParentClass();
            Assert.Throws<InvalidOperationException>(() => Cloneable<ParentClass>.ShallowCloneTo(testClass, testClass));
        }


        [Test]
        public void CloneToClass()
        {
            var source = new CloneTestClass { FirstName = "John", LastName = "Doe" };
            var destination = new CloneTestClass();

            Cloneable<CloneTestClass>.CloneTo(source, destination);

            Assert.AreEqual(source.FirstName, destination.FirstName);
            Assert.AreEqual(source.LastName, destination.LastName);
        }

        [Test]
        public void ShallowCloneToClass()
        {
            var source = new CloneTestClass { FirstName = "John", LastName = "Doe" };
            var destination = new CloneTestClass();

            Cloneable<CloneTestClass>.ShallowCloneTo(source, destination);

            Assert.AreEqual(source.FirstName, destination.FirstName);
            Assert.AreEqual(source.LastName, destination.LastName);
        }

        [Test]
        public void CloneToArray()
        {
            var destinationClass = new CloneTestClass();

            var source = new[] {new CloneTestClass {FirstName = "John", LastName = "Doe"}};
            var destination = new[] { destinationClass };

            Cloneable<CloneTestClass[]>.CloneTo(source, destination);
            
            Assert.IsTrue(ReferenceEquals(destinationClass, destination[0]));
            Assert.AreEqual("John", destinationClass.FirstName);
            Assert.AreEqual("Doe", destinationClass.LastName);
        }


        [Test]
        public void ShallowCloneToArray()
        {
            var destinationClass = new CloneTestClass();

            var source = new[] { new CloneTestClass { FirstName = "John", LastName = "Doe" } };
            var destination = new[] { destinationClass };

            Cloneable<CloneTestClass[]>.ShallowCloneTo(source, destination);

            Assert.IsTrue(ReferenceEquals(source[0], destination[0]));
        }

        [Test]
        public void CloneToArrayOfDifferingLengthsWillNotWork()
        {
            var source = new CloneTestClass[2];
            var destination = new CloneTestClass[3];

            Assert.Throws<InvalidOperationException>(() => Cloneable<CloneTestClass[]>.CloneTo(source, destination));
        }

        [Test]
        public void ShallowCloneToArrayOfDifferingLengthsWillNotWork()
        {
            var source = new CloneTestClass[2];
            var destination = new CloneTestClass[3];

            Assert.Throws<InvalidOperationException>(() => Cloneable<CloneTestClass[]>.ShallowCloneTo(source, destination));
        }
        
        [Test]
        public void CloneToObjectHierarchy()
        {
            var source = new ParentClass();
            var sourceChild = new ChildClass { Name = "John Doe" };

            source.Child = sourceChild;

            var destination = new ParentClass();
            var destinationChild = new ChildClass();

            destination.Child = destinationChild;

            Cloneable<ParentClass>.CloneTo(source, destination);

            Assert.IsTrue(object.ReferenceEquals(destinationChild, destination.Child));
            Assert.AreEqual(sourceChild.Name, destinationChild.Name);
        }

        [Test]
        public void CloneToObjectHierarchyWithNullDestinationChild()
        {
            var source = new ParentClass();
            var sourceChild = new ChildClass { Name = "John Doe" };

            source.Child = sourceChild;

            var destination = new ParentClass();

            Cloneable<ParentClass>.CloneTo(source, destination);

            Assert.IsFalse(object.ReferenceEquals(sourceChild, destination.Child));
            Assert.AreEqual(sourceChild.Name, destination.Child.Name);
        }

        [Test]
        public void CloneToObjectHierarchyWithNullSourceChild()
        {
            var source = new ParentClass();

            var destination = new ParentClass();
            var destinationChild = new ChildClass {Name = "Jim Cox"};

            Cloneable<ParentClass>.CloneTo(source, destination);

            Assert.IsNull(destination.Child);
        }

        [Test]
        public void ShallowCloneToObjectHierarchy()
        {
            var source = new ParentClass();
            var sourceChild = new ChildClass { Name = "John Doe" };

            source.Child = sourceChild;

            var destination = new ParentClass();
            var destinationChild = new ChildClass();

            destination.Child = destinationChild;

            Cloneable<ParentClass>.ShallowCloneTo(source, destination);

            Assert.IsTrue(object.ReferenceEquals(sourceChild, destination.Child));
        }

        [Test]
        public void ShallowCloneToObjectHierarchyWithNullDestinationChild()
        {
            var source = new ParentClass();
            var sourceChild = new ChildClass { Name = "John Doe" };

            source.Child = sourceChild;

            var destination = new ParentClass();

            Cloneable<ParentClass>.ShallowCloneTo(source, destination);

            Assert.IsTrue(object.ReferenceEquals(sourceChild, destination.Child));
            Assert.AreEqual(sourceChild.Name, destination.Child.Name);
        }

        [Test]
        public void ShallowCloneToObjectHierarchyWithNullSourceChild()
        {
            var source = new ParentClass();

            var destination = new ParentClass();
            var destinationChild = new ChildClass { Name = "Jim Cox" };

            Cloneable<ParentClass>.ShallowCloneTo(source, destination);

            Assert.IsNull(destination.Child);
        }

        private void Clone(ParentClass[] source, ParentClass[] destination, bool isShallow, IDictionary<object, object> map)
        {
            if(destination.Length != source.Length)
                throw new ArgumentException("Destination array must be the same length as the source array.", "destination");

            if(isShallow != true)
            {
                ParentClass[] clone = (ParentClass[]) source.Clone();
                for (int i = 0; i < source.Length; i++)
                {
                    if (map.ContainsKey(source[i]))
                        clone[i] = (ParentClass) map[source[i]];
                    else
                    {
                        var newParent = new ParentClass();
                        map.Add(source[i], newParent);

                        Clone(source[i], newParent, false, map);
                    }
                }

                Array.Copy(clone, destination, clone.Length);
            }
            else
                Array.Copy(source, destination, source.Length);
        }

        private void Clone(ParentClass source, ParentClass destination, bool isShallow, IDictionary<object, object> map)
        {
            if (isShallow != true)
            {
                if (source.Child == null)
                    destination.Child = null;
                else
                {
                    if (map.ContainsKey(source.Child))
                        destination.Child = (ChildClass) map[source.Child];
                    else
                    {
                        var newChild = new ChildClass();
                        map.Add(source.Child, newChild);
                        destination.Child = newChild;

                        Clone(source.Child, newChild, false, map);
                    }
                }
            }
            else
                destination.Child = source.Child;
        }

        private void Clone(ChildClass source, ChildClass destination, bool isShallow, IDictionary<object, object> map)
        {
        }
    }
}
