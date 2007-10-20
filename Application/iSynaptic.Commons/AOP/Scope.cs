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

        protected Scope() : this(true)
        {
        }

        internal Scope(bool checkNested)
        {
            if (checkNested && GetCurrent() != null)
                ThrowNestedScopeNotAllowed();

            SetCurrent(this as T);
        }

        protected void ThrowNestedScopeNotAllowed()
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
            if(_Disposed != true)
                Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            SetCurrent(null);
            _Disposed = true;
        }

        protected static void SetCurrent(T current)
        {
            _Current = current;
        }

        protected static T GetCurrent()
        {
            if (_Current != null)
                return _Current;

            return null;
        }
    }
}
