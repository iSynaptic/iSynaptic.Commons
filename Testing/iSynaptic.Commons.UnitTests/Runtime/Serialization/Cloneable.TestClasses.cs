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
