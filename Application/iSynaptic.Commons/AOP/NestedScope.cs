using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public abstract class NestedScope<T> : IDisposable where T : NestedScope<T>
    {
        [ThreadStatic]
        private static Stack<T> _ThreadInstances = null;
        private static Stack<T> _AppDomainInstances = null;

        private static Proc _CreateDefaultScopeHandler = null;

        private T _Parent = null;
        private bool _Disposed = false;
        private ScopeBounds _Bounds = ScopeBounds.Thread;

        protected NestedScope() : this(ScopeBounds.Thread)
        {
        }

        protected NestedScope(ScopeBounds bounds)
        {
            if (Enum.IsDefined(typeof(ScopeBounds), bounds) != true)
                throw new ArgumentOutOfRangeException("bounds");

            if (bounds == ScopeBounds.AppDomain)
            {
                if (AppDomainInstances.Count > 0)
                    Parent = AppDomainInstances.Peek();

                AppDomainInstances.Push(this as T);
            }
            else if (bounds == ScopeBounds.Thread)
            {
                if (ThreadInstances.Count > 0)
                    Parent = ThreadInstances.Peek();

                if (Parent == null && AppDomainInstances.Count > 0)
                    Parent = AppDomainInstances.Peek();

                ThreadInstances.Push(this as T);
            }

            _Bounds = bounds;
        }

        private static Stack<T> AppDomainInstances
        {
            get
            {
                if (_AppDomainInstances == null)
                    _AppDomainInstances = new Stack<T>();

                return _AppDomainInstances;
            }
            set { _AppDomainInstances = value; }
        }

        private static Stack<T> ThreadInstances
        {
            get
            {
                if (_ThreadInstances == null)
                    _ThreadInstances = new Stack<T>();

                return _ThreadInstances;
            }
            set { _ThreadInstances = value; }
        }

        protected static Proc CreateDefaultScopeHandler
        {
            get { return _CreateDefaultScopeHandler; }
            set { _CreateDefaultScopeHandler = value; }
        }

        protected static T InternalCurrent
        {
            get
            {
                if (AppDomainInstances.Count <= 0 && ThreadInstances.Count <= 0 && CreateDefaultScopeHandler != null)
                    CreateDefaultScopeHandler();
                    
                if (ThreadInstances.Count > 0)
                    return ThreadInstances.Peek();
                else if (AppDomainInstances.Count > 0)
                    return AppDomainInstances.Peek();
                else
                    return null;
            }
        }

        protected T Parent
        {
            get { return _Parent; }
            private set { _Parent = value; }
        }

        public void Dispose()
        {
            if (_Disposed != true)
            {
                OnDisposing();

                if (_Bounds == ScopeBounds.AppDomain)
                {
                    T currentAppDomainInstance = AppDomainInstances.Pop();
                    if ((this as T) != currentAppDomainInstance)
                        throw new ApplicationException("NestedScope disposed out of order.");
                }
                else if (_Bounds == ScopeBounds.Thread)
                {
                    T currentThreadInstance = ThreadInstances.Pop();
                    if ((this as T) != currentThreadInstance)
                        throw new ApplicationException("NestedScope disposed out of order.");
                }

                OnDisposed();
                _Disposed = true;
            }
        }

        protected virtual void OnDisposing()
        {
        }

        protected virtual void OnDisposed()
        {
        }
    }
}
