using System;
using System.Collections.Generic;
using System.Text;
using iSynaptic.Commons.Transactions;

namespace iSynaptic.Commons.Transactions
{
    public class SimpleObject
    {
        public SimpleObject()
        {
        }

        public int TestInt { get; set; }
        public Guid TestGuid { get; set; }
        public string TestString { get; set; }
    }
}
