using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons
{
    public abstract class NestableScope<T> : Scope<T> where T : NestableScope<T>
    {
        private T _Parent = null;

        public NestableScope() : this(ScopeBounds.Thread)
        {
        }

        public NestableScope(ScopeBounds bounds)
            : base(bounds, false)
        {
            Initialize();
        }

        protected override void Initialize()
        {
            T current = GetCurrentScope();
            
            if (current != null && current.Bounds == ScopeBounds.Thread && Bounds == ScopeBounds.AppDomain)
                ThrowCannotNestAppDomainScopeInThreadScope();

            if(current != null && current.Bounds == this.Bounds)
                _Parent = current;

            SetCurrentScope(this as T);
        }

        protected override void Dispose(bool disposing)
        {
            if(Parent != null)
                SetCurrentScope(_Parent);

            base.Dispose(disposing);
        }

        protected virtual void ThrowCannotNestAppDomainScopeInThreadScope()
        {
            throw new ApplicationException("You cannot nest a AppDomain level scope under a thread level scope.");
        }

        protected T Parent
        {
            get { return _Parent; }
        }
    }
}
