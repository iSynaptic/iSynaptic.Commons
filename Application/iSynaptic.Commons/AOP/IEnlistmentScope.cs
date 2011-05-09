using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.AOP
{
    public interface IEnlistmentScope<in T> : IDisposable
    {
        bool IsEnlisted(T item);
        void Enlist(params T[] items);
        void Enlist(IEnumerable<T> items);
    }
}
