using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Transactions
{
    [global::System.Serializable]
    public class TransactionalConcurrencyException : Exception
    {
        public TransactionalConcurrencyException() { }
        public TransactionalConcurrencyException(string message) : base(message) { }
        public TransactionalConcurrencyException(string message, Exception inner) : base(message, inner) { }
        protected TransactionalConcurrencyException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
