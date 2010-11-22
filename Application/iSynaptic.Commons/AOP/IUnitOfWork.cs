using System.Collections.Generic;
using System;

namespace iSynaptic.Commons.AOP
{
    public interface IUnitOfWork<T> : IEnlistmentScope<T>
    {
        void Complete();
    }
}