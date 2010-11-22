using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace iSynaptic.Commons.AOP
{
    public abstract class EnlistmentScope<T, S> : Scope<S>, IEnlistmentScope<T>
        where S : EnlistmentScope<T, S>
    {
        private List<T> _Items = new List<T>();

        public EnlistmentScope(ScopeBounds bounds, ScopeNesting nesting) : base(bounds, nesting)
        {
        }

        public bool IsEnlisted(T item)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            if (_Items.Contains(item))
                return true;

            return false;
        }

        public void Enlist(params T[] items)
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            Enlist((IEnumerable<T>)items);
        }

        public virtual void Enlist(IEnumerable<T> items)
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

        protected ICollection<T> Items
        {
            get { return _Items; }
        }
    }
}
