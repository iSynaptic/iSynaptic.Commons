using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Transactions
{
    [Serializable]
    public class TransactionalConcurrencyException : Exception
    {
        public TransactionalConcurrencyException() { }
    }
}
