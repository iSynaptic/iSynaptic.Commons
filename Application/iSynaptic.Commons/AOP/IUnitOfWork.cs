using System.Collections.Generic;
using System;

namespace iSynaptic.Commons.AOP
{
    public interface IUnitOfWork<T> : IDisposable
    {
        bool IsEnlisted(T item);
        void Enlist(params T[] items);
        void Enlist(IEnumerable<T> items);

        void Complete();
    }
}