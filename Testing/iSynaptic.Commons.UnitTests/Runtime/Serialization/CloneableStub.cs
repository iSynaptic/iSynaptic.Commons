using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.UnitTests.Runtime.Serialization
{
    public class CloneableStub : IEquatable<CloneableStub>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public int YearsOfService { get; set; }
        public double BigNumber { get; set; }
        public decimal ADecimal { get; set; }

        public bool Equals(CloneableStub other)
        {
            return Id == other.Id &&
                Name == other.Name &&
                BirthDate == other.BirthDate &&
                YearsOfService == other.YearsOfService &&
                BigNumber == other.BigNumber &&
                ADecimal == other.ADecimal;
        }

        public void Test(CloneableStub other)
        {
            ADecimal = other.ADecimal;
        }
    }
}
