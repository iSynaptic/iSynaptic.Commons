using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    //public abstract class NestedScope<T> : Scope<T> where T : NestedScope<T>
    //{
    //    private ScopeBounds _Bounds = ScopeBounds.Thread;

    //    private static T _AppDomainCurrent = null;

    //    protected NestedScope() : this(ScopeBounds.Thread)
    //    {
    //    }

    //    protected NestedScope(ScopeBounds bounds)
    //    {
    //        if (Enum.IsDefined(typeof(ScopeBounds), bounds) != true)
    //            throw new ArgumentOutOfRangeException("bounds");

    //        if (bounds == ScopeBounds.AppDomain)
    //        {
    //            if (AppDomainInstances.Count > 0)
    //                Parent = AppDomainInstances.Peek();

    //            AppDomainInstances.Push(this as T);
    //        }
    //        else if (bounds == ScopeBounds.Thread)
    //        {
    //            if (ThreadInstances.Count > 0)
    //                Parent = ThreadInstances.Peek();

    //            if (Parent == null && AppDomainInstances.Count > 0)
    //                Parent = AppDomainInstances.Peek();

    //            ThreadInstances.Push(this as T);
    //        }

    //        _Bounds = bounds;
    //    }

    //    public void Dispose()
    //    {
    //        if (_Disposed != true)
    //            Dispose(true);
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //    }
    //}
}
