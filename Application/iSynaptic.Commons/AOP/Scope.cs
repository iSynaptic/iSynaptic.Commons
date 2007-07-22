using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public abstract class Scope<T> : IDisposable where T : Scope<T>
    {
        [ThreadStatic]
        private static T _ThreadInstance = null;
        private static T _AppDomainInstance = null;

        private static Proc _CreateDefaultScopeHandler = null;

        private bool _Disposed = false;
        private ScopeBounds _Bounds = ScopeBounds.Thread;

        protected Scope() : this(ScopeBounds.Thread)
        {
        }

        protected Scope(ScopeBounds bounds)
        {
            if (Enum.IsDefined(typeof(ScopeBounds), bounds) != true)
                throw new ArgumentOutOfRangeException("bounds");

            if (bounds == ScopeBounds.AppDomain)
            {
                if (_AppDomainInstance != null)
                    throw new ApplicationException("Nested scopes are not allowed.");

                _AppDomainInstance = this as T;
            }
            else if (bounds == ScopeBounds.Thread)
            {
                if(_AppDomainInstance != null || _ThreadInstance != null)
                    throw new ApplicationException("Nested scopes are not allowed.");

                _ThreadInstance = this as T;
            }

            _Bounds = bounds;
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
                if (_AppDomainInstance == null && _ThreadInstance == null && CreateDefaultScopeHandler != null)
                    CreateDefaultScopeHandler();
                    
                if (_ThreadInstance != null)
                    return _ThreadInstance;
                else if (_AppDomainInstance != null)
                    return _AppDomainInstance;
                else
                    return null;
            }
        }

        public void Dispose()
        {
            if (_Disposed != true)
            {
                OnDisposing();

                if (_Bounds == ScopeBounds.AppDomain)
                    _AppDomainInstance = null;
                else if (_Bounds == ScopeBounds.Thread)
                    _ThreadInstance = null;

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
