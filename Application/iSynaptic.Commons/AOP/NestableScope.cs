using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public abstract class NestableScope<T> : IDisposable where T : NestableScope<T>
    {
        private bool _Disposed = false;

        private T _Parent = null;
        private T _Child = null;

        private ScopeBounds _Bounds = ScopeBounds.Thread;

        [ThreadStatic]
        private static T _ThreadCurrent = null;
        private static T _Current = null;

        protected NestableScope() : this(ScopeBounds.Thread)
        {
        }

        protected NestableScope(ScopeBounds bounds)
        {
            if (Enum.IsDefined(typeof(ScopeBounds), bounds) != true)
                throw new ArgumentOutOfRangeException("bounds");

            if (bounds == ScopeBounds.AppDomain)
            {
                if (_ThreadCurrent != null)
                    ThrowCannotNestAppDomainScopeUnderThreadScope();

                if (_Current != null)
                {
                    _Parent = _Current;
                    _Parent.RegisterChild(this as T);
                }

                _Current = this as T;
            }
            else if (bounds == ScopeBounds.Thread)
            {
                if (_ThreadCurrent != null)
                    _Parent = _ThreadCurrent;

                _ThreadCurrent = this as T;
            }
            else
                throw new ArgumentOutOfRangeException("bounds");

            _Bounds = bounds;
        }

        private void ThrowCannotNestAppDomainScopeUnderThreadScope()
        {
            Exception ex = GetAppDomainNestedUnderThreadException();
            if (ex != null)
                throw ex;
            else
                throw new InvalidOperationException("An AppDomain bound nestable scope cannot be nested under a thread bound nestable scope.");
        }

        protected virtual Exception GetAppDomainNestedUnderThreadException()
        {
            return null;
        }

        public void Dispose()
        {
            if (_Disposed != true)
            {
                DisposeChild(true);
                Dispose(true);

                _Disposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            DisposeChild(true);

            if (_Bounds == ScopeBounds.AppDomain)
            {
                if (_Parent != null)
                {
                    _Current = _Parent;
                    _Parent.UnregisterChild(this as T);
                    
                }
                else
                    _Current = null;
            }
            else if (_Bounds == ScopeBounds.Thread)
            {
                if (_Parent != null)
                {
                    _ThreadCurrent = _Parent;
                    _Parent.UnregisterChild(this as T);
                }
                else
                    _ThreadCurrent = null;
            }

            _Parent = null;
        }

        private void DisposeChild(bool disposing)
        {
            if (_Child != null)
                _Child.Dispose(disposing);
        }

        protected void RegisterChild(T child)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            if (_Child != null && _Child != child)
                throw new InvalidOperationException("A different child scope is already registered.");

            _Child = child;
        }

        protected void UnregisterChild(T child)
        {
            if (child == null)
                throw new ArgumentNullException("child");

            if (_Child != child)
                throw new InvalidOperationException("The provided scope was not registered as a child.");

            _Child = null;
        }

        protected static T GetCurrent()
        {
            if (_ThreadCurrent != null)
                return _ThreadCurrent;
            else if (_Current != null)
                return _Current;
            else
                return null;
        }

        protected bool Disposed
        {
            get { return _Disposed; }
        }
    }
}
