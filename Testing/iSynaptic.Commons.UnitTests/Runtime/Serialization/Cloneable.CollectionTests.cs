// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace iSynaptic.Commons.Runtime.Serialization
{
    public partial class CloneableTests
    {
        [Test]
        public void CloneListOfValueType()
        {
            List<int> source = new List<int> { 1, 2, 3, 4, 5 };

            Assert.IsTrue(Cloneable<List<int>>.CanClone());
            Assert.IsTrue(Cloneable<List<int>>.CanShallowClone());

            List<int> clone = source.Clone();
            List<int> shallowClone = source.ShallowClone();

            Assert.IsFalse(ReferenceEquals(source, clone));
            Assert.IsFalse(ReferenceEquals(source, shallowClone));

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

            Assert.IsFalse(ReferenceEquals(source, clone));
            Assert.IsFalse(ReferenceEquals(source, shallowClone));

            Assert.AreEqual(1, clone.Count);
            Assert.AreEqual(1, shallowClone.Count);

            Assert.IsFalse(ReferenceEquals(source[0], clone[0]));
            Assert.IsTrue(ReferenceEquals(source[0], shallowClone[0]));

            Assert.AreEqual("John", clone[0].FirstName);
            Assert.AreEqual("Doe", clone[0].LastName);
        }

        [Test]
        public void ClonePrimitiveArray()
        {
            int[] ints = new int[] { 1, 2, 3, 4, 5 };

            Assert.IsTrue(Cloneable<int[]>.CanClone());
            Assert.IsTrue(Cloneable<int[]>.CanShallowClone());

            int[] clonedInts = Cloneable<int[]>.Clone(ints);
            int[] shallowClonedInts = Cloneable<int[]>.ShallowClone(ints);

            Assert.IsFalse(ReferenceEquals(ints, clonedInts));
            Assert.IsFalse(ReferenceEquals(ints, shallowClonedInts));

            Assert.IsTrue(clonedInts.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
            Assert.IsTrue(shallowClonedInts.SequenceEqual(new int[] { 1, 2, 3, 4, 5 }));
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
        public void CloneToClassArray()
        {
            var destinationClass = new CloneTestClass();

            var source = new[] { new CloneTestClass { FirstName = "John", LastName = "Doe" } };
            var destination = new[] { destinationClass };

            Cloneable<CloneTestClass[]>.CloneTo(source, destination);

            Assert.IsTrue(ReferenceEquals(destinationClass, destination[0]));
            Assert.AreEqual("John", destinationClass.FirstName);
            Assert.AreEqual("Doe", destinationClass.LastName);
        }

        [Test]
        public void ShallowCloneToClassArray()
        {
            var destinationClass = new CloneTestClass();

            var source = new[] { new CloneTestClass { FirstName = "John", LastName = "Doe" } };
            var destination = new[] { destinationClass };

            Cloneable<CloneTestClass[]>.ShallowCloneTo(source, destination);

            Assert.IsTrue(ReferenceEquals(source[0], destination[0]));
        }

        [Test]
        public void CloneToStructArray()
        {
            var source = new[] { new CloneTestStruct { FirstName = "John", LastName = "Doe" } };
            var destination = new CloneTestStruct[1];

            source.CloneTo(destination);

            Assert.AreEqual("John", destination[0].FirstName);
            Assert.AreEqual("Doe", destination[0].LastName);
        }

        [Test]
        public void ShallowCloneToStructArray()
        {
            var source = new[] { new CloneTestStruct { FirstName = "John", LastName = "Doe" } };
            var destination = new CloneTestStruct[1];

            source.ShallowCloneTo(destination);

            Assert.AreEqual("John", destination[0].FirstName);
            Assert.AreEqual("Doe", destination[0].LastName);
        }

        [Test]
        public void CloneToMultidimentionalClassArray()
        {
            CloneTestClass[,] source = new[,]
            {
                {
                    new CloneTestClass { FirstName = "John", LastName = "Doe" },
                    new CloneTestClass { FirstName = "Jane", LastName = "Smith"}
                }
            };

            var dest1 = new CloneTestClass();
            var dest2 = new CloneTestClass();

            var destination = new[,]
            {
                {
                    dest1,
                    dest2
                }
            };

            source.CloneTo(destination);

            Assert.IsTrue(ReferenceEquals(dest1, destination[0, 0]));
            Assert.IsTrue(ReferenceEquals(dest2, destination[0, 1]));

            Assert.AreEqual("John", destination[0, 0].FirstName);
            Assert.AreEqual("Doe", destination[0, 0].LastName);

            Assert.AreEqual("Jane", destination[0, 1].FirstName);
            Assert.AreEqual("Smith", destination[0, 1].LastName);
        }

        [Test]
        public void ShallowCloneToMultidimentionalClassArray()
        {
            var source1 = new CloneTestClass { FirstName = "John", LastName = "Doe" };
            var source2 = new CloneTestClass { FirstName = "Jane", LastName = "Smith" };

            CloneTestClass[,] source = new[,]
            {
                {
                    source1,
                    source2
                }
            };

            var destination = new[,]
            {
                {
                    new CloneTestClass(),
                    new CloneTestClass()
                }
            };

            source.ShallowCloneTo(destination);

            Assert.IsTrue(ReferenceEquals(source1, destination[0, 0]));
            Assert.IsTrue(ReferenceEquals(source2, destination[0, 1]));
        }

        [Test]
        public void CloneToMultidimentionalStructArray()
        {
            var dest1 = new CloneTestStructWithClonableClassField { TestClass = new CloneTestClass() };
            var dest2 = new CloneTestStructWithClonableClassField { TestClass = new CloneTestClass() };

            var source = new[,]
            {
                {
                    new CloneTestStructWithClonableClassField { TestClass = new CloneTestClass { FirstName = "John", LastName = "Doe" } },
                    new CloneTestStructWithClonableClassField { TestClass = new CloneTestClass { FirstName = "Jane", LastName = "Smith" } }
                }
            };

            var dest = new[,]
            {
                {
                    dest1,
                    dest2
                }
            };

            var clone = source.CloneTo(dest);

            Assert.IsTrue(ReferenceEquals(clone[0, 0].TestClass, dest1.TestClass));
            Assert.IsTrue(ReferenceEquals(clone[0, 1].TestClass, dest2.TestClass));

            Assert.AreEqual("John", clone[0, 0].TestClass.FirstName);
            Assert.AreEqual("Doe", clone[0, 0].TestClass.LastName);

            Assert.AreEqual("Jane", clone[0, 1].TestClass.FirstName);
            Assert.AreEqual("Smith", clone[0, 1].TestClass.LastName);
        }

        [Test]
        public void ShallowCloneToMultidimentionalStructArray()
        {
            var source1 = new CloneTestStructWithClonableClassField { TestClass = new CloneTestClass { FirstName = "John", LastName = "Doe" } };
            var source2 = new CloneTestStructWithClonableClassField { TestClass = new CloneTestClass { FirstName = "Jane", LastName = "Smith" } };

            var source = new[,]
            {
                {
                    source1,
                    source2
                }
            };

            var dest = new CloneTestStructWithClonableClassField[1, 2];

            var clone = source.ShallowCloneTo(dest);

            Assert.IsTrue(ReferenceEquals(clone[0, 0].TestClass, source1.TestClass));
            Assert.IsTrue(ReferenceEquals(clone[0, 1].TestClass, source2.TestClass));
        }

        [Test]
        public void CloneToJaggedClassArray()
        {
            var source = new[]
            {
                new[] {new CloneTestClass { FirstName = "John", LastName = "Doe" }},
                new[] {new CloneTestClass { FirstName = "Jane", LastName = "Smith"}}
            };

            var dest1 = new CloneTestClass();
            var dest2 = new CloneTestClass();

            var destination = new[]
            {
                new[] {dest1}, 
                new[] {dest2}
            };

            source.CloneTo(destination);

            Assert.IsTrue(ReferenceEquals(dest1, destination[0][0]));
            Assert.IsTrue(ReferenceEquals(dest2, destination[1][0]));

            Assert.AreEqual("John", destination[0][0].FirstName);
            Assert.AreEqual("Doe", destination[0][0].LastName);

            Assert.AreEqual("Jane", destination[1][0].FirstName);
            Assert.AreEqual("Smith", destination[1][0].LastName);
        }

        [Test]
        public void ShallowCloneToJaggedClassArray()
        {
            var source1 = new CloneTestClass { FirstName = "John", LastName = "Doe" };
            var source2 = new CloneTestClass { FirstName = "Jane", LastName = "Smith" };

            var source = new[]
            {
                new[] {source1},
                new[] {source2}
            };

            var destination = new[]
            {
                new[] {new CloneTestClass()}, 
                new[] {new CloneTestClass()}
            };

            source.ShallowCloneTo(destination);

            Assert.IsTrue(ReferenceEquals(source1, destination[0][0]));
            Assert.IsTrue(ReferenceEquals(source2, destination[1][0]));
        }

        [Test]
        public void CloneToJaggedStructArray()
        {
            var source = new[]
            {
                new[] {new CloneTestStructWithClonableClassField { TestClass = new CloneTestClass { FirstName = "John", LastName = "Doe" }}},
                new[] {new CloneTestStructWithClonableClassField { TestClass = new CloneTestClass { FirstName = "Jane", LastName = "Smith"}}}
            };

            var dest1 = new CloneTestClass();
            var dest2 = new CloneTestClass();

            var destination = new[]
            {
                new[] {new CloneTestStructWithClonableClassField { TestClass = dest1}}, 
                new[] {new CloneTestStructWithClonableClassField { TestClass = dest2}}
            };

            var clone = source.CloneTo(destination);

            Assert.IsTrue(ReferenceEquals(dest1, clone[0][0].TestClass));
            Assert.IsTrue(ReferenceEquals(dest2, clone[1][0].TestClass));

            Assert.AreEqual("John", clone[0][0].TestClass.FirstName);
            Assert.AreEqual("Doe", clone[0][0].TestClass.LastName);

            Assert.AreEqual("Jane", clone[1][0].TestClass.FirstName);
            Assert.AreEqual("Smith", clone[1][0].TestClass.LastName);
        }

        [Test]
        public void ShallowCloneToJaggedStructArray()
        {
            var source1 = new CloneTestClass { FirstName = "John", LastName = "Doe" };
            var source2 = new CloneTestClass { FirstName = "Jane", LastName = "Smith" };

            var source = new[]
            {
                new[] {new CloneTestStructWithClonableClassField { TestClass = source1}},
                new[] {new CloneTestStructWithClonableClassField { TestClass = source2}}
            };

            var destination = new[]
            {
                new[] {new CloneTestStructWithClonableClassField()}, 
                new[] {new CloneTestStructWithClonableClassField()}
            };

            var clone = source.ShallowCloneTo(destination);

            Assert.IsTrue(ReferenceEquals(source1, destination[0][0].TestClass));
            Assert.IsTrue(ReferenceEquals(source2, destination[1][0].TestClass));
        }

        [Test]
        public void CloneToClassArrayOfDifferingLengthsWillNotWork()
        {
            var source = new CloneTestClass[2];
            var destination = new CloneTestClass[3];

            Assert.Throws<InvalidOperationException>(() => Cloneable<CloneTestClass[]>.CloneTo(source, destination));
        }

        [Test]
        public void ShallowCloneToClassArrayOfDifferingLengthsWillNotWork()
        {
            var source = new CloneTestClass[2];
            var destination = new CloneTestClass[3];

            Assert.Throws<InvalidOperationException>(() => Cloneable<CloneTestClass[]>.ShallowCloneTo(source, destination));
        }

        [Test]
        public void CloneToStructArrayOfDifferingLengthsWillNotWork()
        {
            var source = new CloneTestStruct[2];
            var destination = new CloneTestStruct[3];

            Assert.Throws<InvalidOperationException>(() => Cloneable<CloneTestStruct[]>.CloneTo(source, destination));
        }

        [Test]
        public void ShallowCloneToStructArrayOfDifferingLengthsWillNotWork()
        {
            var source = new CloneTestStruct[2];
            var destination = new CloneTestStruct[3];

            Assert.Throws<InvalidOperationException>(() => Cloneable<CloneTestStruct[]>.ShallowCloneTo(source, destination));
        }
    }
}
