using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.Transactions
{
    public interface ITransactional<T> where T : class, ITransactional<T>
    {
        T Duplicate();
    }
}
