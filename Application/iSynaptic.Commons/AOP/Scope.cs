using System;

namespace iSynaptic.Commons.AOP
{
    public abstract class Scope<T> : IDisposable where T : Scope<T>
    {
        private static T _CurrentAppDomainScope = null;

        [ThreadStatic]
        private static T _CurrentThreadScope = null;

        protected Scope(ScopeBounds bounds, ScopeNesting nesting)
        {
            if (bounds.IsDefined() != true)
                throw new ArgumentOutOfRangeException("bounds");

            if(nesting.IsDefined() != true)
                throw new ArgumentOutOfRangeException("nesting");

            Bounds = bounds;
            Nesting = nesting;

            Initialize();
        }

        private void Initialize()
        {
            if (Nesting == ScopeNesting.Allowed)
            {
                T current = GetCurrentScope();

                if (current != null && current.Bounds == ScopeBounds.Thread && Bounds == ScopeBounds.AppDomain)
                    OnThrowCannotNestAppDomainScopeInThreadScope();

                if (current != null && current.Bounds == Bounds)
                    Parent = current;
            }
            else if (GetCurrentScope() != null)
                OnNestedScopesNotAllowed();

            SetCurrentScope(this as T);
        }

        protected static T GetCurrentScope()
        {
            if (_CurrentThreadScope != null)
                return _CurrentThreadScope;

            return _CurrentAppDomainScope;
        }

        protected void SetCurrentScope(T scope)
        {
            if (scope.Bounds == ScopeBounds.AppDomain)
                _CurrentAppDomainScope = scope;
            else
                _CurrentThreadScope = scope;
        }

        protected virtual void OnNestedScopesNotAllowed()
        {
            throw new ApplicationException("Nested scopes are not allowed.");
        }

        protected virtual void OnThrowCannotNestAppDomainScopeInThreadScope()
        {
            throw new ApplicationException("You cannot nest a AppDomain level scope under a Thread level scope.");
        }

        public void Dispose()
        {
            if (Disposed != true)
            {
                Dispose(true);
                Disposed = true;

                GC.SuppressFinalize(this);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if(Nesting == ScopeNesting.Allowed && Parent != null)
                SetCurrentScope(Parent);

            if (Bounds == ScopeBounds.AppDomain && _CurrentAppDomainScope == this)
                _CurrentAppDomainScope = null;
            else if (Bounds == ScopeBounds.Thread && _CurrentThreadScope == this)
                _CurrentThreadScope = null;
        }

        protected ScopeBounds Bounds { get; private set; }
        protected ScopeNesting Nesting { get; private set; }

        protected bool Disposed { get; private set; }

        protected T Parent { get; private set; }
    }
}
