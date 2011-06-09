using System;
using System.Collections.Generic;
using System.Linq;

namespace iSynaptic.Commons.AOP
{
    public class DisposableContext : EnlistmentScope<IDisposable, DisposableContext>
    {
        private readonly CompositeDisposable _CompositeDisposable = new CompositeDisposable();

        public DisposableContext() : this(ScopeBounds.Thread, ScopeNesting.Allowed)
        {
        }

        public DisposableContext(ScopeBounds bounds, ScopeNesting nesting) : base(bounds, nesting)
        {
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (Parent != null)
                    {
                        Parent.Enlist(Items);
                        Items.Clear();
                    }
                    else
                    {
                        _CompositeDisposable.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected override ICollection<IDisposable> Items
        {
            get { return _CompositeDisposable; }
        }

        public static DisposableContext Current
        {
            get { return GetCurrentScope(); }
        }
    }
}
