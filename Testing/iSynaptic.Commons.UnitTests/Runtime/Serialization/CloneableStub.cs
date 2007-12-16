using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.Runtime.Serialization
{
    public class CloneableStub
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int YearsOfService { get; set; }
    }
}
