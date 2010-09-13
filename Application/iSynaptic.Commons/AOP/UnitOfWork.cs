using System;
using System.Collections.Generic;
using System.Text;

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
            if(Disposed)
                throw new ObjectDisposedException(GetType().Name);

            if (Items.Contains(item))
                return true;

            if (Parent != null && Parent.IsEnlisted(item))
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

            Items.AddRange(items);
        } 

        #endregion

        protected override void Dispose(bool disposing)
        {
            try
            {
                if(Disposed != true)
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
            if(Disposed)
                throw new ObjectDisposedException(GetType().Name);

            if(Completed)
                throw new InvalidOperationException("Unit of work has already been completed.");

            Completed = true;
            Process(Items);
        }

        protected bool Completed { get; private set; }

        protected List<T> Items
        {
            get { return _Items ?? (_Items = new List<T>()); }
        }
    }
}
