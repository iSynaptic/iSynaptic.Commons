using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.Runtime.Serialization
{
    public class NotCloneableStub
    {
        public string CloneableString { get; set; }
        public IntPtr NotCloneablePointer { get; set; }
    }
}
