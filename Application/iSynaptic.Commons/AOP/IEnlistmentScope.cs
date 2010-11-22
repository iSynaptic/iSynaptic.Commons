using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.AOP
{
    public interface IEnlistmentScope<T> : IDisposable
    {
        bool IsEnlisted(T item);
        void Enlist(params T[] items);
        void Enlist(IEnumerable<T> items);
    }
}
