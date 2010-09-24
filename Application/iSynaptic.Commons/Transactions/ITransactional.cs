using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iSynaptic.Commons.Runtime.Serialization;

namespace iSynaptic.Commons.Transactions
{
    [CloneReferenceOnly]
    public interface ITransactional<T>
    {
        T Value { get; set; }
    }
}
