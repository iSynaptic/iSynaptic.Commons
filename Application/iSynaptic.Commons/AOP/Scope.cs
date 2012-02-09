// The MIT License
// 
// Copyright (c) 2011 Jordan E. Terrell
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
            Bounds = Guard.MustBeDefined(bounds, "bounds");
            Nesting = Guard.MustBeDefined(nesting, "nesting");

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
            return _CurrentThreadScope ?? _CurrentAppDomainScope;
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
