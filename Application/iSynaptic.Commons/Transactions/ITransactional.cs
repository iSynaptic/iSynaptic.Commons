using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.Transactions
{
    public interface ITransactional<T>
    {
        T Value { get; set; }
    }
}
