using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.AOP
{
    public abstract class Scope<T> : IDisposable where T : Scope<T>
    {
        private static T _CurrentAppDomainScope = null;

        [ThreadStatic]
        private static T _CurrentThreadScope = null;

        protected Scope() : this(ScopeBounds.Thread)
        {
        }

        protected Scope(ScopeBounds bounds) : this(bounds, true)
        {
        }

        internal Scope(ScopeBounds bounds, bool shouldInitialize)
        {
            if (bounds.IsDefined() != true)
                throw new ArgumentOutOfRangeException("bounds");

            Bounds = bounds;

            if (shouldInitialize)
                Initialize();
        }

        protected virtual void Initialize()
        {
            if (GetCurrentScope() != null)
                NestedScopesNotAllowed();

            SetCurrentScope(this as T);
        }

        protected virtual void NestedScopesNotAllowed()
        {
            throw new ApplicationException("Nested scopes are not allowed.");
        }

        public static T GetCurrentScope()
        {
            if (_CurrentAppDomainScope != null)
                return _CurrentAppDomainScope;
            else
                return _CurrentThreadScope;
        }

        protected void SetCurrentScope(T scope)
        {
            if (scope.Bounds == ScopeBounds.AppDomain)
                _CurrentAppDomainScope = scope;
            else
                _CurrentThreadScope = scope;
        }

        public void Dispose()
        {
            if (Disposed != true)
            {
                Dispose(true);
                Disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Bounds == ScopeBounds.AppDomain && _CurrentAppDomainScope == this)
                _CurrentAppDomainScope = null;
            else if (Bounds == ScopeBounds.Thread && _CurrentThreadScope == this)
                _CurrentThreadScope = null;
        }

        protected bool Disposed { get; private set; }
        protected ScopeBounds Bounds { get; private set; }
    }
}
