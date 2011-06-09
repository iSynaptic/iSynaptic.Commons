using System;
using System.Collections.Generic;
using System.Threading;
using iSynaptic.Commons.Collections.Generic;

namespace iSynaptic.Commons.AOP
{
    public abstract class EnlistmentScope<TItem, TScope> : Scope<TScope>, IEnlistmentScope<TItem>
        where TScope : EnlistmentScope<TItem, TScope>
    {
        private List<TItem> _Items = null;

        protected EnlistmentScope(ScopeBounds bounds, ScopeNesting nesting) : base(bounds, nesting)
        {
        }

        public virtual bool IsEnlisted(TItem item)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            return Items.Contains(item);
        }

        public void Enlist(params TItem[] items)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            Enlist((IEnumerable<TItem>)items);
        }

        public virtual void Enlist(IEnumerable<TItem> items)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            Items.AddRange(items);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (Disposed != true)
                    Items.Clear();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected virtual ICollection<TItem> Items
        {
            get
            {
                if (_Items == null)
                    Interlocked.CompareExchange(ref _Items, new List<TItem>(), null);

                return _Items;
            }
        }
    }
}
