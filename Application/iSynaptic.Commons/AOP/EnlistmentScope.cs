using System;
using System.Collections.Generic;

namespace iSynaptic.Commons.AOP
{
    public abstract class EnlistmentScope<TItem, TScope> : Scope<TScope>, IEnlistmentScope<TItem>
        where TScope : EnlistmentScope<TItem, TScope>
    {
        private readonly List<TItem> _Items = new List<TItem>();

        protected EnlistmentScope(ScopeBounds bounds, ScopeNesting nesting) : base(bounds, nesting)
        {
        }

        public bool IsEnlisted(TItem item)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            return _Items.Contains(item);
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

            _Items.AddRange(items);
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

        protected ICollection<TItem> Items
        {
            get { return _Items; }
        }
    }
}
