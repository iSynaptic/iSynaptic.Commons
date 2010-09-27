using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iSynaptic.Commons.AOP
{
    public class DisposableContext : NestableScope<DisposableContext>
    {
        private List<IDisposable> _Disposables = null;

        public bool IsEnlisted(IDisposable disposable)
        {
            if (Disposables.Contains(disposable))
                return true;

            if (Parent != null && Parent.IsEnlisted(disposable))
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

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (Parent != null)
                {
                    Parent.Enlist(Disposables);
                    Disposables.Clear();
                }
                else
                {
                    Action<IDisposable> dispose = d => d.Dispose();
                    List<Exception> exceptions = new List<Exception>();

                    Disposables.ForEach(dispose.CatchExceptions(exceptions));
                    Disposables.Clear();

                    if (exceptions.Count > 0)
                        throw new AggregateException("Exception(s) occured during disposal.", exceptions);
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected List<IDisposable> Disposables
        {
            get { return _Disposables ?? (_Disposables = new List<IDisposable>()); }
        }
    }
}
