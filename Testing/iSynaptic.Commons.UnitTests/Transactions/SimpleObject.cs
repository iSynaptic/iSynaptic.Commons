using System;
using System.Collections.Generic;
using System.Text;
using iSynaptic.Commons.Transactions;

namespace iSynaptic.Commons.UnitTests.Transactions
{
    public class SimpleObject : ITransactional<SimpleObject>
    {
        public SimpleObject()
        {
        }

        public int TestInt { get; set; }
        public Guid TestGuid { get; set; }
        public string TestString { get; set; }

        SimpleObject ITransactional<SimpleObject>.Duplicate()
        {
            return new SimpleObject
            {
                TestInt = this.TestInt,
                TestGuid = this.TestGuid,
                TestString = this.TestString
            };
        }
    }
}
