using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public abstract class Scope<T> : IDisposable where T : Scope<T>
    {
        private bool _Disposed = false;

        [ThreadStatic]
        private static T _Current = null;

        protected Scope()
        {
            if (GetCurrent() != null)
                ThrowNestedScopeNotAllowed();

            _Current = this as T;
        }

        private void ThrowNestedScopeNotAllowed()
        {
            Exception ex = GetNestedScopeException();
            if (ex != null)
                throw ex;
            else
                throw new ApplicationException("Scope<T> cannot be nested. Use NestedScope<T>.");
        }

        protected virtual Exception GetNestedScopeException()
        {
            return null;
        }

        public void Dispose()
        {
            if (_Disposed != true)
            {
                Dispose(true);
                _Disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            _Current = null;
        }

        protected static T GetCurrent()
        {
            if (_Current != null)
                return _Current;

            return null;
        }

        protected bool Disposed
        {
            get { return _Disposed; }
        }
    }
}
