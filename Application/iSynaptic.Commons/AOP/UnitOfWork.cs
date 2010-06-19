using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.AOP
{
    public abstract class UnitOfWork<U, T> : NestableScope<U>, IUnitOfWork<T>
        where U : NestableScope<U>, IUnitOfWork<T>
    {
        private List<T> _Items = null;

        public UnitOfWork() : base(ScopeBounds.Thread)
        {
        }

        #region Enlistment Methods

        public bool IsEnlisted(T item)
        {
            if (Items.Contains(item))
                return true;

            if (Parent != null && Parent.IsEnlisted(item))
                return true;

            return false;
        }

        public void Enlist(params T[] items)
        {
            Enlist((IEnumerable<T>)items);
        }

        public virtual void Enlist(IEnumerable<T> items)
        {
            Items.AddRange(items);
        } 

        #endregion

        protected override void Dispose(bool disposing)
        {
            try
            {
                Items.Clear();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected abstract void Process(IEnumerable<T> items);

        public void Complete()
        {
            Process(Items);
        }

        protected List<T> Items
        {
            get { return _Items ?? (_Items = new List<T>()); }
        }
    }
}
