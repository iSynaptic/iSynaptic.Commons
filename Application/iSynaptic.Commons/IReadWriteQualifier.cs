using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public interface IReadWriteQualifier<Q, T>
    {
        T this[Q qualifier] { get; set; }
    }
}
