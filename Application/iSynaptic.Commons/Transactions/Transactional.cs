using System;
using System.Collections.Generic;
using System.Transactions;

using iSynaptic.Commons.Collections.Generic;
using iSynaptic.Commons.Runtime.Serialization;
using iSynaptic.Commons.Threading;

namespace iSynaptic.Commons.Transactions
{
    public class Transactional<T> : TransactionalBase<T>
    {
        public Transactional()
            : this(default(T))
        {
        }

        public Transactional(T current) : base(current)
        {
        }
    }
}
