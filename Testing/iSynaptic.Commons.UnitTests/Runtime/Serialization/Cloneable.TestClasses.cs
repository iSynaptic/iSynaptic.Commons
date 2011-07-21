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

namespace iSynaptic.Commons.Runtime.Serialization
{
	public partial class CloneableTests
	{
        private class ClassWithReferenceToTypeThatHasCloneReferenceOnlyAttribute
        {
            public CloneOnlyByReferenceClass Reference { get; set; }
        }

        public interface ICloneByReferenceOnly
        {
        }

        [CloneReferenceOnly]
        private class CloneOnlyByReferenceClass
        {
            public string Name { get; set; }
        }

        public class WithInterfaceReferenceClass
        {
            public ICloneByReferenceOnly Reference { get; set; }
        }

        public class WithInterfaceClass : ICloneByReferenceOnly
        {
            public string Name { get; set; }
        }

        public class ClassWithCollectionViaInterface<T>
        {
            public IEnumerable<T> Collection { get; set; }
        }

        private struct StructWithReferenceToSelfReferencingClass
        {
            public CloneTestClass Class { get; set; }
        }

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

        private struct CloneTestStructWithClonableClassField
        {
            public CloneTestClass TestClass { get; set; }
        }

        private class ClassWithFuncField
        {
            public Func<int> CannotCloneThis { get; set; }
        }

        private class DerivedClassWithFuncField : ClassWithFuncField
        {
        }

        private class ClassWithDelegateField
        {
            public Action CannotCloneThis { get; set; }
        }

        private class DerivedClassWithDelegateField : ClassWithDelegateField
        {
        }

        private struct StructWithFuncField
        {
            public Func<int> CannotCloneThis { get; set; }
        }

        private class ReferenceOnlyCloneTestClass
        {
            [CloneReferenceOnly]
            public ReferenceOnlyCloneTestClass InnerClass = null;
        }

        private class ClassWithFuncArray
        {
            public Func<int>[] CannotCloneThis { get; set; }
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
            public Func<int> CannotCloneThis { get; set; }
        }

        private class ClassWithNonSerializedIllegalField
        {
            [NonSerialized]
            private IntPtr CannotCloneThis = IntPtr.Zero;

            public string Name { get; set; }
        }

        private class BaseClass
        {
            public string BaseClassName { get; set; }
        }

        private class DerivedClass : BaseClass
        {
            public string DerivedClassName { get; set; }
        }

        private class ClassWithReferenceToBaseClass
        {
            public BaseClass Reference { get; set; }
        }
	}
}
