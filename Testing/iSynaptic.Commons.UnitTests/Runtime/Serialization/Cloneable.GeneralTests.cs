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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace iSynaptic.Commons.Runtime.Serialization
{
    [TestFixture]
    public partial class CloneableTests
    {
        [Test]
        public void CloneClass()
        {
            var source = new CloneTestClass { FirstName = "John", LastName = "Doe" };

            Assert.IsTrue(Cloneable<CloneTestClass>.CanClone());
            Assert.IsTrue(Cloneable<CloneTestClass>.CanShallowClone());

            var clone = Cloneable<CloneTestClass>.Clone(source);
            var shallowClone = Cloneable<CloneTestClass>.ShallowClone(source);

            Assert.IsFalse(ReferenceEquals(source, clone));
            Assert.IsFalse(ReferenceEquals(source, shallowClone));

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
        public void CloneStructWithSelfReferencingClass()
        {
            var selfRefClass = new CloneTestClass { FirstName = "John", LastName = "Doe" };
            selfRefClass.InnerClass = selfRefClass;

            var source = new StructWithReferenceToSelfReferencingClass {Class = selfRefClass};

            var clone = Cloneable<StructWithReferenceToSelfReferencingClass>.Clone(source);
            var shallowClone = Cloneable<StructWithReferenceToSelfReferencingClass>.ShallowClone(source);

            Assert.IsFalse(object.ReferenceEquals(source.Class, clone.Class));
            Assert.IsTrue(object.ReferenceEquals(source.Class, shallowClone.Class));

            Assert.IsTrue(object.ReferenceEquals(clone.Class, clone.Class.InnerClass));
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
        public void CannotCloneClassWithIntPtrField()
        {
            Assert.IsFalse(Cloneable<ClassWithFuncField>.CanClone());
            Assert.IsFalse(Cloneable<ClassWithFuncField>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithFuncField>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithFuncField>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithFuncField>.Clone(new ClassWithFuncField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithFuncField>.ShallowClone(new ClassWithFuncField()));
        }

        [Test]
        public void CannotCloneDerivedClassWithIntPtrField()
        {
            Assert.IsFalse(Cloneable<DerivedClassWithFuncField>.CanClone());
            Assert.IsFalse(Cloneable<DerivedClassWithFuncField>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithFuncField>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithFuncField>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithFuncField>.Clone(new DerivedClassWithFuncField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<DerivedClassWithFuncField>.ShallowClone(new DerivedClassWithFuncField()));
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
        public void CannotCloneStructWithFuncField()
        {
            Assert.IsFalse(Cloneable<StructWithFuncField>.CanClone());
            Assert.IsFalse(Cloneable<StructWithFuncField>.CanShallowClone());

            Assert.IsFalse(Cloneable<StructWithFuncField?>.CanClone());
            Assert.IsFalse(Cloneable<StructWithFuncField?>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithFuncField>.Clone(new StructWithFuncField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithFuncField>.ShallowClone(new StructWithFuncField()));
            
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithFuncField?>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithFuncField?>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithFuncField?>.Clone(new StructWithFuncField()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<StructWithFuncField?>.ShallowClone(new StructWithFuncField()));
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
            Assert.IsFalse(Cloneable<ClassWithFuncArray>.CanClone());
            Assert.IsFalse(Cloneable<ClassWithFuncArray>.CanShallowClone());

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithFuncArray>.Clone(null));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithFuncArray>.ShallowClone(null));

            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithFuncArray>.Clone(new ClassWithFuncArray()));
            Assert.Throws<InvalidOperationException>(() => Cloneable<ClassWithFuncArray>.ShallowClone(new ClassWithFuncArray()));
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
        public void CloneToWillOnAValueType()
        {
            var testClass = new CloneTestClass();

            var source = new CloneTestStructWithClonableClassField {TestClass = new CloneTestClass {FirstName = "John", LastName = "Doe"}};
            var dest = new CloneTestStructWithClonableClassField {TestClass = testClass};

            var clone = source.CloneTo(dest);

            Assert.IsTrue(ReferenceEquals(testClass, clone.TestClass));
            Assert.AreEqual("John", clone.TestClass.FirstName);
            Assert.AreEqual("Doe", clone.TestClass.LastName);
        }

        [Test]
        public void ShallowCloneToWillAValueType()
        {
            var testClass = new CloneTestClass {FirstName = "John", LastName = "Doe"};

            var source = new CloneTestStructWithClonableClassField { TestClass = testClass };
            var dest = new CloneTestStructWithClonableClassField {TestClass = new CloneTestClass()};

            var clone = source.ShallowCloneTo(dest);

            Assert.IsTrue(ReferenceEquals(testClass, clone.TestClass));
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

            var destination = new ParentClass {Child = new ChildClass {Name = "John Doe"}};

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

            var destination = new ParentClass {Child = new ChildClass {Name = "John Doe"}};

            Cloneable<ParentClass>.ShallowCloneTo(source, destination);

            Assert.IsNull(destination.Child);
        }

        [Test]
        public void CloneClassWithReferenceToTypeThatHasCloneReferenceOnlyAttribute()
        {
            var ro = new CloneOnlyByReferenceClass();
            var source = new ClassWithReferenceToTypeThatHasCloneReferenceOnlyAttribute {Reference = ro};

            var clone = source.Clone();
            Assert.IsTrue(ReferenceEquals(ro, clone.Reference));
        }

        [Test]
        public void CloneClassWithClassReferenceViaInterface()
        {
            var wic = new WithInterfaceClass {Name = "John Doe"};
            var source = new WithInterfaceReferenceClass {Reference = wic};

            var clone = source.Clone();

            Assert.IsFalse(ReferenceEquals(clone.Reference, wic));
            Assert.IsAssignableFrom(typeof (WithInterfaceClass), clone.Reference);
            Assert.AreEqual("John Doe", ((WithInterfaceClass)clone.Reference).Name);
        }

        [Test]
        public void CloneClassWithCollectionReferenceViaInterface()
        {
            var source = new ClassWithCollectionViaInterface<string>();
            var clone = source.Clone();

            Assert.IsNull(clone.Collection);

            source.Collection = new string[] {"Hello", "World!"};
            clone = source.Clone();
            Assert.AreEqual(clone.Collection.GetType(), typeof(string[]));
            Assert.IsTrue(clone.Collection.SequenceEqual(new []{"Hello", "World!"}));

            source.Collection = new Collection<string> { "Hello", "World!" };
            clone = source.Clone();
            Assert.AreEqual(clone.Collection.GetType(), typeof(Collection<string>));
            Assert.IsTrue(clone.Collection.SequenceEqual(new[] { "Hello", "World!" }));

            source.Collection = new List<string> { "Hello", "World!" };
            clone = source.Clone();
            Assert.AreEqual(clone.Collection.GetType(), typeof(List<string>));
            Assert.IsTrue(clone.Collection.SequenceEqual(new[] { "Hello", "World!" }));
        }

        [Test]
        public void CloneDerivedClassThroughBaseClassReference()
        {
            var source = new ClassWithReferenceToBaseClass {Reference = new BaseClass {BaseClassName = "Foo"}};
            var clone = source.Clone();

            Assert.AreEqual("Foo", clone.Reference.BaseClassName);

            source.Reference = new DerivedClass {BaseClassName = "Bar", DerivedClassName = "Baz"};

            clone = source.Clone();
            var derivedClone = clone.Reference as DerivedClass;

            Assert.IsNotNull(derivedClone);
            Assert.AreEqual("Bar", derivedClone.BaseClassName);
            Assert.AreEqual("Baz", derivedClone.DerivedClassName);
        }
    }
}
