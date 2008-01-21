using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MbUnit.Framework;

using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons.UnitTests.Runtime.Serialization
{
    [TestFixture]
    public class CloneableTests : BaseTestFixture
    {
        #region Cloneable Primitive Tests

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

        #endregion

        [Test]
        public void CannotCloneIntPtr()
        {
            Assert.IsFalse(Cloneable<IntPtr>.CanClone());
            Assert.IsFalse(Cloneable<IntPtr>.CanShallowClone());

            AssertThrows<InvalidOperationException>(() => Cloneable<IntPtr>.Clone(IntPtr.Zero));
            AssertThrows<InvalidOperationException>(() => Cloneable<IntPtr>.ShallowClone(IntPtr.Zero));
        }

        [Test]
        public void CannotCloneDelegate()
        {
            Assert.IsFalse(Cloneable<Action>.CanClone());
            Assert.IsFalse(Cloneable<Action>.CanShallowClone());

            AssertThrows<InvalidOperationException>(() => Cloneable<Action>.Clone(null));
            AssertThrows<InvalidOperationException>(() => Cloneable<Action>.ShallowClone(null));
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

        private class CloneArrayTestClass
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public CloneArrayTestClass InnerClass { get; set; }
        }

        private struct CloneArrayTestStruct
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        [Test]
        public void CloneClass()
        {
            var source = new CloneArrayTestClass { FirstName = "John", LastName = "Doe" };

            Assert.IsTrue(Cloneable<CloneArrayTestClass>.CanClone());
            Assert.IsTrue(Cloneable<CloneArrayTestClass>.CanShallowClone());

            var clone = Cloneable<CloneArrayTestClass>.Clone(source);
            var shallowClone = Cloneable<CloneArrayTestClass>.ShallowClone(source);

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
            var clone = Cloneable<CloneArrayTestClass>.Clone(null);
            var shallowClone = Cloneable<CloneArrayTestClass>.ShallowClone(null);

            Assert.IsNull(clone);
            Assert.IsNull(shallowClone);
        }

        [Test]
        public void CloneSelfReferencingClass()
        {
            var source = new CloneArrayTestClass { FirstName = "John", LastName = "Doe" };
            source.InnerClass = source;

            var clone = Cloneable<CloneArrayTestClass>.Clone(source);
            var shallowClone = Cloneable<CloneArrayTestClass>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source, clone));
            Assert.IsFalse(object.ReferenceEquals(source, shallowClone));

            Assert.IsTrue(object.ReferenceEquals(clone, clone.InnerClass));
            Assert.IsTrue(object.ReferenceEquals(shallowClone, shallowClone.InnerClass));
        }

        [Test]
        public void CloneStruct()
        {
            var source = new CloneArrayTestStruct { FirstName = "John", LastName = "Doe" };

            Assert.IsTrue(Cloneable<CloneArrayTestStruct>.CanClone());
            Assert.IsTrue(Cloneable<CloneArrayTestStruct>.CanShallowClone());

            var clone = Cloneable<CloneArrayTestStruct>.Clone(source);
            var shallowClone = Cloneable<CloneArrayTestStruct>.ShallowClone(source);

            Assert.AreEqual("John", clone.FirstName);
            Assert.AreEqual("Doe", clone.LastName);

            Assert.AreEqual("John", shallowClone.FirstName);
            Assert.AreEqual("Doe", shallowClone.LastName);
        }

        [Test]
        public void CloneClassArray()
        {
            var source = new CloneArrayTestClass[]
            {
                new CloneArrayTestClass { FirstName = "John", LastName = "Doe" },
                new CloneArrayTestClass { FirstName = "Jane", LastName = "Smith"}
            };

            Assert.IsTrue(Cloneable<CloneArrayTestClass[]>.CanClone());
            Assert.IsTrue(Cloneable<CloneArrayTestClass[]>.CanShallowClone());

            var clones = Cloneable<CloneArrayTestClass[]>.Clone(source);
            var shallowClones = Cloneable<CloneArrayTestClass[]>.ShallowClone(source);

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
            var source = new CloneArrayTestStruct[]
            {
                new CloneArrayTestStruct { FirstName = "John", LastName = "Doe" },
                new CloneArrayTestStruct { FirstName = "Jane", LastName = "Smith"}
            };

            Assert.IsTrue(Cloneable<CloneArrayTestStruct[]>.CanClone());
            Assert.IsTrue(Cloneable<CloneArrayTestStruct[]>.CanShallowClone());

            var clones = Cloneable<CloneArrayTestStruct[]>.Clone(source);
            var shallowClones = Cloneable<CloneArrayTestStruct[]>.ShallowClone(source);

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
            CloneArrayTestClass[,] source = new CloneArrayTestClass[,]
            {
                {
                    new CloneArrayTestClass { FirstName = "John", LastName = "Doe" },
                    new CloneArrayTestClass { FirstName = "Jane", LastName = "Smith"}
                }
            };

            source[0, 1].InnerClass = source[0, 1];

            Assert.IsTrue(Cloneable<CloneArrayTestClass[,]>.CanClone());
            Assert.IsTrue(Cloneable<CloneArrayTestClass[,]>.CanShallowClone());

            var clone = Cloneable<CloneArrayTestClass[,]>.Clone(source);
            var shallowClone = Cloneable<CloneArrayTestClass[,]>.ShallowClone(source);

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
            CloneArrayTestStruct[,] source = new CloneArrayTestStruct[,]
            {
                {
                    new CloneArrayTestStruct { FirstName = "John", LastName = "Doe" },
                    new CloneArrayTestStruct { FirstName = "Jane", LastName = "Smith"}
                }
            };

            Assert.IsTrue(Cloneable<CloneArrayTestStruct[,]>.CanClone());
            Assert.IsTrue(Cloneable<CloneArrayTestStruct[,]>.CanShallowClone());

            var clone = Cloneable<CloneArrayTestStruct[,]>.Clone(source);
            var shallowClone = Cloneable<CloneArrayTestStruct[,]>.ShallowClone(source);

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
            var source = new CloneArrayTestClass[][]
            {
                new CloneArrayTestClass[]
                {
                    new CloneArrayTestClass { FirstName = "John", LastName = "Doe" },
                    new CloneArrayTestClass { FirstName = "Jane", LastName = "Smith"}
                }

            };

            Assert.IsTrue(Cloneable<CloneArrayTestClass[][]>.CanClone());
            Assert.IsTrue(Cloneable<CloneArrayTestClass[][]>.CanShallowClone());

            var clone = Cloneable<CloneArrayTestClass[][]>.Clone(source);
            var shallowClone = Cloneable<CloneArrayTestClass[][]>.ShallowClone(source);

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
            var source = new CloneArrayTestStruct[][]
            {
                new CloneArrayTestStruct[]
                {
                    new CloneArrayTestStruct { FirstName = "John", LastName = "Doe" },
                    new CloneArrayTestStruct { FirstName = "Jane", LastName = "Smith"}
                }

            };

            Assert.IsTrue(Cloneable<CloneArrayTestStruct[][]>.CanClone());
            Assert.IsTrue(Cloneable<CloneArrayTestStruct[][]>.CanShallowClone());

            var clone = Cloneable<CloneArrayTestStruct[][]>.Clone(source);
            var shallowClone = Cloneable<CloneArrayTestStruct[][]>.ShallowClone(source);

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

        //[Test]
        //public void CannotCloneComplexType()
        //{
        //    Assert.IsFalse(Cloneable<NotCloneableStub>.CanClone());
        //    Assert.IsFalse(Cloneable<NotCloneableStub>.CanShallowClone());

        //    try
        //    {
        //        Assert.IsNull(Cloneable<NotCloneableStub>.Clone(new NotCloneableStub()));
        //        Assert.Fail("Exception was not thrown.");
        //    }
        //    catch (InvalidOperationException) { }

        //    try
        //    {
        //        Assert.IsNull(Cloneable<NotCloneableStub>.ShallowClone(new NotCloneableStub()));
        //        Assert.Fail("Exception was not thrown.");
        //    }
        //    catch (InvalidOperationException) { }
        //}

        //[Test]
        //public void CannotCloneDerivedComplexType()
        //{
        //    Assert.IsFalse(Cloneable<DerivedNotCloneableStub>.CanClone());
        //    Assert.IsFalse(Cloneable<DerivedNotCloneableStub>.CanShallowClone());

        //    try
        //    {
        //        Assert.IsNull(Cloneable<DerivedNotCloneableStub>.Clone(new DerivedNotCloneableStub()));
        //        Assert.Fail("Exception was not thrown.");
        //    }
        //    catch (InvalidOperationException) { }

        //    try
        //    {
        //        Assert.IsNull(Cloneable<DerivedNotCloneableStub>.ShallowClone(new DerivedNotCloneableStub()));
        //        Assert.Fail("Exception was not thrown.");
        //    }
        //    catch (InvalidOperationException) { }
        //}

        //[Test]
        //public void CloneComplexType()
        //{
        //    CloneableStub stub = new CloneableStub
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "Name",
        //        BirthDate = DateTime.Now,
        //        YearsOfService = 25,
        //        Ints = new int[]{ 1, 2, 3}
        //    };

        //    CloneableStub clonedStub = Cloneable<CloneableStub>.Clone(stub);

        //    Assert.IsTrue(Cloneable<CloneableStub>.CanClone());
        //    Assert.IsTrue(Cloneable<CloneableStub>.CanShallowClone());

        //    Assert.IsNotNull(clonedStub);
        //    Assert.IsFalse(object.ReferenceEquals(stub, clonedStub));

        //    Assert.IsTrue(clonedStub.Equals(stub));
        //}

        //[Test]
        //public void CloneDerivedComplexType()
        //{
        //    DerivedCloneableStub stub = new DerivedCloneableStub
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "Name",
        //        BirthDate = DateTime.Now,
        //        YearsOfService = 25,
        //        DerivedName = "DerivedName"
        //    };

        //    DerivedCloneableStub clonedStub = Cloneable<DerivedCloneableStub>.Clone(stub);

        //    Assert.IsTrue(Cloneable<DerivedCloneableStub>.CanClone());
        //    Assert.IsTrue(Cloneable<DerivedCloneableStub>.CanShallowClone());

        //    Assert.IsNotNull(clonedStub);
        //    Assert.IsFalse(object.ReferenceEquals(stub, clonedStub));

        //    Assert.IsTrue(stub.Equals(clonedStub));
        //}

        //[Test]
        //public void CloneSelfReferencing()
        //{
        //    Guid id = Guid.NewGuid();
        //    DateTime birthDate = DateTime.Now.AddDays(-1);

        //    var stub = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };
        //    stub.FirstChild = stub;
        //    stub.SecondChild = stub;

        //    var clone = Cloneable<CloneableStub>.Clone(stub);

        //    Assert.IsFalse(object.ReferenceEquals(stub, clone));
        //    Assert.IsFalse(object.ReferenceEquals(stub.FirstChild, clone.FirstChild));
        //    Assert.IsFalse(object.ReferenceEquals(stub.SecondChild, clone.SecondChild));

        //    Assert.IsTrue(object.ReferenceEquals(clone, clone.FirstChild));
        //    Assert.IsTrue(object.ReferenceEquals(clone, clone.SecondChild));
        //}

        //[Test]
        //public void CloneBidirectionalReferences()
        //{
        //    Guid id = Guid.NewGuid();
        //    DateTime birthDate = DateTime.Now.AddDays(-1);

        //    var parent = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };
        //    var child = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };

        //    parent.FirstChild = child;
        //    parent.SecondChild = child;

        //    child.FirstChild = parent;
        //    child.SecondChild = parent;

        //    var clonedParent = Cloneable<CloneableStub>.Clone(parent);

        //    Assert.IsFalse(object.ReferenceEquals(parent, clonedParent));

        //    Assert.IsFalse(object.ReferenceEquals(parent.FirstChild, clonedParent.FirstChild));
        //    Assert.IsFalse(object.ReferenceEquals(parent.SecondChild, clonedParent.SecondChild));

        //    var clonedChild = clonedParent.FirstChild;

        //    Assert.IsTrue(object.ReferenceEquals(clonedChild.FirstChild, clonedParent));
        //    Assert.IsTrue(object.ReferenceEquals(clonedChild.SecondChild, clonedParent));
        //}

        //[Test]
        //public void CloneMultipleReferencesToSameArray()
        //{
        //    Guid id = Guid.NewGuid();
        //    DateTime birthDate = DateTime.Now.AddDays(-1);

        //    var child = new CloneableStub[]
        //    {
        //        new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 },
        //        new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 }
        //    };

        //    var source = new CloneableStub[][]
        //    {
        //        child,
        //        child
        //    };

        //    var clone = Cloneable<CloneableStub[][]>.Clone(source);

        //    Assert.IsFalse(object.ReferenceEquals(source, clone));
        //    Assert.IsFalse(object.ReferenceEquals(clone[0], child));
        //    Assert.IsFalse(object.ReferenceEquals(clone[1], child));

        //    Assert.IsTrue(object.ReferenceEquals(clone[0], clone[1]));
        //}

        //[Test]
        //public void ShallowCloneComplexType()
        //{
        //    Guid id = Guid.NewGuid();
        //    DateTime birthDate = DateTime.Now.AddDays(-1);

        //    var parent = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };
        //    var child = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };

        //    parent.FirstChild = child;
        //    parent.SecondChild = child;

        //    child.FirstChild = parent;
        //    child.SecondChild = parent;

        //    var clonedParent = Cloneable<CloneableStub>.ShallowClone(parent);

        //    Assert.IsFalse(object.ReferenceEquals(parent, clonedParent));

        //    Assert.IsTrue(object.ReferenceEquals(parent.FirstChild, clonedParent.FirstChild));
        //    Assert.IsTrue(object.ReferenceEquals(parent.SecondChild, clonedParent.SecondChild));

        //    var clonedChild = clonedParent.FirstChild;

        //    Assert.IsFalse(object.ReferenceEquals(clonedChild.FirstChild, clonedParent));
        //    Assert.IsFalse(object.ReferenceEquals(clonedChild.SecondChild, clonedParent));

        //    Assert.IsTrue(object.ReferenceEquals(clonedChild.FirstChild, parent));
        //    Assert.IsTrue(object.ReferenceEquals(clonedChild.SecondChild, parent));
        //}

        //[Test]
        //public void CloneReferenceOnly()
        //{
        //    Guid id = Guid.NewGuid();
        //    DateTime birthDate = DateTime.Now.AddDays(-1);

        //    var stub = new CloneableStub { Id = id, BirthDate = birthDate, Name = "Stub 1", YearsOfService = 1 };
        //    stub.ReferenceOnlyClone = stub;

        //    var clonedStub = Cloneable<CloneableStub>.Clone(stub);

        //    Assert.IsFalse(object.ReferenceEquals(stub, clonedStub));
        //    Assert.IsTrue(object.ReferenceEquals(stub, clonedStub.ReferenceOnlyClone));
        //}

        //[Test]
        //public void ShallowCloneNull()
        //{
        //    string result = Cloneable<string>.ShallowClone(null);
        //    Assert.IsNull(result);
        //}
    }
}
