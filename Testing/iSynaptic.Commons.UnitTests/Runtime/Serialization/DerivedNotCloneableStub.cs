using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.Runtime.Serialization
{
    public class DerivedNotCloneableStub : NotCloneableStub
    {
        public Guid CloneableGuid { get; set; }
    }
}
