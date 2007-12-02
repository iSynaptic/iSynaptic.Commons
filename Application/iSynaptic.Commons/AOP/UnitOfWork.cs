using System;
using System.Collections.Generic;
using System.Text;

namespace iSynaptic.Commons.AOP
{
    public abstract class UnitOfWork<T> : NestableScope<UnitOfWork<T>>
    {
        private List<T> _Items = null;

        public UnitOfWork() : base(ScopeBounds.Thread)
        {
        }

        public void Register(params T[] items)
        {
            Register((IEnumerable<T>) items);
        }

        public virtual void Register(IEnumerable<T> items)
        {
            Items.AddRange(items);
        }

        public abstract void Complete();

        protected List<T> Items
        {
            get { return _Items ?? (_Items = new List<T>()); }
        }

        public static UnitOfWork<T> Current
        {
            get { return GetCurrentScope(); }
        }
    }
}
