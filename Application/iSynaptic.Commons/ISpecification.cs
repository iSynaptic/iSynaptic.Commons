using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
    }
}
