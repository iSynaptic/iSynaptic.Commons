using System;
using System.Collections.Generic;
using System.Text;

using iSynaptic.Commons.Extensions;

namespace iSynaptic.Commons.AOP
{
    public abstract class UnitOfWork<U, T> : NestableScope<U>
        where U : UnitOfWork<U, T>
    {
        private List<T> _Items = null;
        private List<IDisposable> _Disposables = null;

        public UnitOfWork() : base(ScopeBounds.Thread)
        {
        }

        #region Enlistment Methods

        public bool IsEnlisted(IDisposable disposable)
        {
            if (Disposables.Contains(disposable))
                return true;

            if (Parent != null && Parent.IsEnlisted(disposable))
                return true;

            return false;
        }

        public bool IsEnlisted(T item)
        {
            if (Items.Contains(item))
                return true;

            if (Parent != null && Parent.IsEnlisted(item))
                return true;

            return false;
        }

        public void Enlist(params IDisposable[] disposables)
        {
            Enlist((IEnumerable<IDisposable>)disposables);
        }

        public virtual void Enlist(IEnumerable<IDisposable> disposables)
        {
            Disposables.AddRange(disposables);
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
                if (Parent != null)
                {
                    Parent.Enlist(Disposables);

                    Disposables.Clear();
                    Items.Clear();
                }
                else
                {
                    Action<IDisposable> dispose = d => d.Dispose();
                    List<Exception> exceptions = new List<Exception>();

                    Disposables.ForEach(dispose.CatchExceptions(exceptions));
                    Disposables.Clear();
                    Items.Clear();

                    if (exceptions.Count > 0)
                        throw new ContainerException("Exception(s) occured during disposal.", exceptions);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        public abstract void Complete();

        protected List<T> Items
        {
            get { return _Items ?? (_Items = new List<T>()); }
        }

        protected List<IDisposable> Disposables
        {
            get { return _Disposables ?? (_Disposables = new List<IDisposable>()); }
        }

        public static U Current
        {
            get { return GetCurrentScope(); }
        }
    }
}
