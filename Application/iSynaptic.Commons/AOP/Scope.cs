using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public abstract class Scope<T> : IDisposable where T : Scope<T>
    {
        private bool _Disposed = false;

        [ThreadStatic]
        private static T _CurrentScope = null;

        protected Scope()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            if (_CurrentScope != null)
                NestedScopesNotAllowed();

            SetCurrentScope(this as T);
        }

        protected virtual void NestedScopesNotAllowed()
        {
            throw new ApplicationException("Nested scopes are not allowed.");
        }

        protected static T GetCurrentScope()
        {
            return _CurrentScope;
        }

        protected void SetCurrentScope(T scope)
        {
            _CurrentScope = scope;
        }

        public void Dispose()
        {
            if (_Disposed != true)
            {
                _CurrentScope = null;
                _Disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            _CurrentScope = null;
        }

        protected bool Disposed
        {
            get { return _Disposed; }
        }
    }
}
