using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.Runtime.Serialization
{
    public class DerivedCloneableStub : CloneableStub, IEquatable<DerivedCloneableStub>
    {
        public string DerivedName { get; set; }

        public bool Equals(DerivedCloneableStub other)
        {
            return DerivedName == other.DerivedName &&
                ((CloneableStub)this).Equals(other);
        }
    }
}
